using System;
using System.Collections.Generic;
using System.Text;
using Marten;
using Marten.Services;

namespace Ubora.Domain.Tests
{

    public abstract class DocumentSessionIntegrationFixture : StoreIntegrationFixture
    {
        private readonly Lazy<IDocumentSession> _session;


        protected DocumentSessionIntegrationFixture()
        {
            _session = new Lazy<IDocumentSession>(() => theStore.DirtyTrackedSession());
        }

        protected IDocumentSession Session => _session.Value;

        public override void Dispose()
        {
            if (_session.IsValueCreated)
            {
                _session.Value.Dispose();
            }

            base.Dispose();
        }
    }
}
