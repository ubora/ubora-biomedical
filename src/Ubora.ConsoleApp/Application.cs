using Marten;
using Marten.Linq.SoftDeletes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using Marten.Events.Projections.Async;
using Marten.Services;
using Ubora.Domain.Projects;

namespace Ubora.ConsoleApp
{
    internal class Application
    {
        private readonly IDocumentStore _documentStore;

        public Application(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        /// <remarks>
        /// Should probably use async daemon in the future: https://jeremydmiller.com/2016/08/04/offline-event-processing-in-marten-with-the-new-async-daemon/
        /// </remarks>
        public void Run()
        {
            IReadOnlyCollection<Guid> ids = null;

            Console.WriteLine("I hope you have the application turned off so no new writes can happen... And you have built the console application from the latest Ubora.Domain... Press any key to start");
            Console.ReadLine();
            Console.WriteLine("Starting reaggregaton. I hope you have the application turned off so no new writes can happen... And you have built the console application from the latest domain... Press any key to start!");

            Console.WriteLine("Running initial reaggregation without saving to check for exceptions...");
            using (var documentSession = _documentStore.OpenSession())
            {
                ids = documentSession.Query<Project>().Where(x => x.MaybeDeleted()).Select(x => x.Id).ToList();

                foreach (var id in ids)
                {
                    documentSession.Events.AggregateStream<Project>(id);
                }
            }
            Console.WriteLine("No exceptions in the projections.");

            Console.WriteLine("Running reaggregation with persisted updates to documents...");
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            using (var transactionScope =
                new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot, Timeout = TimeSpan.FromMinutes(10) }))
            {
                using (var documentSession = _documentStore.OpenSession(new SessionOptions
                {
                    EnlistInAmbientTransactionScope = true,
                    OwnsTransactionLifecycle = false
                }))
                {
                    foreach (var id in ids)
                    {
                        var project = documentSession.Events.AggregateStream<Project>(id);
                        documentSession.Store(project);
                    }

                    documentSession.SaveChanges();
                }

                transactionScope.Complete();
            }
            stopwatch.Stop();

            Console.WriteLine($"Reaggregation has successfully finished! Number of documents affected: {ids.Count}, Milliseconds elapsed: {stopwatch.ElapsedMilliseconds}");
            Console.ReadLine();
        }
    }
}
