using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Marten;
using Marten.Linq.SoftDeletes;
using Marten.Services;
using Microsoft.Extensions.DependencyInjection;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects._Commands;

namespace Ubora.MarkdownToQuillDeltaMigrator
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            if (string.IsNullOrWhiteSpace(args[0]))
            {
                throw new InvalidOperationException("Connection string not set.");
            }

            CompositionRoot(connectionString: args[0])
                .GetService<Application>()
                .Run();

            Console.ReadLine();
        }

        private static IServiceProvider CompositionRoot(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddNodeServices();

            var autofacContainerBuilder = new ContainerBuilder();
            autofacContainerBuilder.RegisterType<Application>();
            autofacContainerBuilder.RegisterType<MarkdownToQuillDeltaConverter>().As<IMarkdownToQuillDeltaConverter>();

            var domainModule = new DomainAutofacModule(connectionString, storageProvider: null);
            autofacContainerBuilder.RegisterModule(domainModule);

            autofacContainerBuilder.Populate(services);
            var autofacContainer = autofacContainerBuilder.Build();

            return new AutofacServiceProvider(autofacContainer);
        }
    }

    internal class Application
    {
        private readonly IDocumentStore _documentStore;
        private readonly ILifetimeScope _rootLifetimeScope;

        public Application(IDocumentStore documentStore, ILifetimeScope rootLifetimeScope)
        {
            _documentStore = documentStore;
            _rootLifetimeScope = rootLifetimeScope;
        }

        public void Run()
        {
            var projectIds = QueryProjectIds().ToList();
            foreach (var projectId in projectIds)
            {
                using (var transactionScope = CreateTransactionScope())
                {
                    using (var documentSession = _documentStore.OpenSession(new SessionOptions
                    {
                        EnlistInAmbientTransactionScope = true,
                        OwnsTransactionLifecycle = false
                    }))
                    {
                        using (var lifetimeScope = _rootLifetimeScope.BeginLifetimeScope(builder =>
                        {
                            builder
                                .RegisterInstance(documentSession)
                                .As<IDocumentSession>()
                                .As<IQuerySession>();
                        }))
                        {
                            lifetimeScope
                                .Resolve<ICommandQueryProcessor>()
                                .Execute(new ConvertProjectWpStepsFromMarkdownToQuillDeltaCommand
                                {
                                    ProjectId = projectId,
                                    Actor = new UserInfo(Guid.Parse("52309cef-225a-4442-aa10-57f9fe03c8ed"), "Kaspar")
                                });
                        }
                    }

                    transactionScope.Complete();
                }
            }

            Console.WriteLine("Conversion complete. Number of projects affected: " + projectIds.Count);

            TransactionScope CreateTransactionScope()
            {
                return new TransactionScope(
                    TransactionScopeOption.Required, 
                    new TransactionOptions
                    {
                        IsolationLevel = IsolationLevel.Snapshot,
                        Timeout = TimeSpan.FromMinutes(1)
                    });
            }
        }

        private IReadOnlyCollection<Guid> QueryProjectIds()
        {
            using (var session = _documentStore.OpenSession())
            {
                var ids = session.Query<Project>()
                    .Where(p => p.MaybeDeleted())
                    .Select(p => p.Id)
                    .ToList();

                // Bonus: Order the projects by last change to the stream.
                var streamStates = ids
                    .Select(id => session.Events.FetchStreamState(id))
                    .OrderByDescending(s => s.LastTimestamp);

                return streamStates.Select(s => s.Id).ToList();
            }
        }
    }
}
