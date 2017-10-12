using System;
using Marten;

namespace Ubora.Domain.Tests
{
    public abstract class DocumentSessionIntegrationFixture : StoreIntegrationFixture
    {
        private Lazy<IDocumentSession> _session;

        protected DocumentSessionIntegrationFixture()
        {
            InitializeSession();
        }

        private void InitializeSession()
        {
            // Use 'LightweightSession' because it does not have any caching, which is suitable for testing. http://jasperfx.github.io/marten/documentation/troubleshoot/
            _session = new Lazy<IDocumentSession>(() => theStore.LightweightSession());
        }

        protected IDocumentSession Session => _session.Value;

        protected virtual void RefreshSession()
        {
            Session.Dispose();
            InitializeSession();
        }

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
