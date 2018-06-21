using Marten;
using Marten.Events.Projections.Async;
using System;
using System.Linq;

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
            var streamIds = _documentSession.Events.QueryAllRawEvents().Select(e => e.StreamId).Distinct();
            foreach (var streamId in streamIds)
            {
                foreach (var inlineProjection in _documentSession.DocumentStore.Events.InlineProjections)
                {
                    var streamEvents = _documentSession.Events.FetchStream(streamId);
                    inlineProjection.Apply(_documentSession, new EventPage(0, 0, streamEvents));
                    _documentSession.SaveChanges();
                }
            }

            _documentSession.SaveChanges();

            Console.WriteLine("Changed!");
            Console.ReadLine();
        }
    }
}
