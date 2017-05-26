using Marten;
using System;
using System.Linq;
using Ubora.Domain.Projects.DeviceClassification;

namespace Ubora.Web
{
    public class Seeder
    {
        private readonly IDocumentSession _documentSession;

        public Seeder(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        internal void SeedIfNecessary()
        {
            var isSeedNecessary = !_documentSession.Query<DeviceClassification>().Any();

            if (isSeedNecessary)
            {
                var deviceClassification = new DeviceClassification();
                deviceClassification.CreateNew();

                _documentSession.Store(deviceClassification);
                _documentSession.SaveChanges();
            }
        }
    }
}
