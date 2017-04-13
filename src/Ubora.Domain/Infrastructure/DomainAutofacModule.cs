using System;
using Autofac;
using Marten;
using Marten.Events;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Domain.Queries;

namespace Ubora.Domain.Infrastructure
{
    public class DomainAutofacModule : Module
    {
        private readonly string _connectionString;

        public DomainAutofacModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var options = new StoreOptions();
            options.Connection(_connectionString);

            options.NameDataLength = 100;

            Action<StoreOptions> configuration = new UboraStoreOptions().Configuration();

            configuration.Invoke(options);

            builder.RegisterInstance(new DocumentStore(options)).SingleInstance();
            builder.Register(x => x.Resolve<DocumentStore>().OpenSession()).As<IDocumentSession>().As<IQuerySession>().InstancePerLifetimeScope();
            builder.Register(x => x.Resolve<IDocumentSession>().Events).As<IEventStore>().InstancePerLifetimeScope();

            builder.RegisterType<EventStreamQuery>().As<IEventStreamQuery>().InstancePerLifetimeScope();
            builder.RegisterType<CommandQueryProcessor>().As<ICommandProcessor>().As<IQueryProcessor>().InstancePerLifetimeScope();
            builder.RegisterType<CreateProjectHandler>().As<ICommandHandler<CreateProject>>().InstancePerLifetimeScope();
        }
    }
}
