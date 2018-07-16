using Marten;
using Marten.Linq.SoftDeletes;
using System;
using System.Linq;
using Ubora.Domain.Projects;

namespace Ubora.ConsoleApp
{
    class Application
    {
        private readonly IDocumentSession _documentSession;

        public Application(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public void Run()
        {
            Console.WriteLine("Reaggregationing...");
            var ids = _documentSession.Query<Project>().Where(x => x.MaybeDeleted()).Select(x => x.Id).ToList();

            foreach (var id in ids)
            {
                var project = _documentSession.Events.AggregateStream<Project>(id);
                _documentSession.Store(project);
            }

            _documentSession.SaveChanges();

            Console.WriteLine("Changed!");
            Console.ReadLine();
        }
    }
}
