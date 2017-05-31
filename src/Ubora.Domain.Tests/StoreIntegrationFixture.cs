using Marten;
using System;
using System.Reflection;
using Xunit;

namespace Ubora.Domain.Tests
{
    public abstract class StoreIntegrationFixture : IDisposable
    {
        private Lazy<IDocumentStore> _store;

        protected StoreIntegrationFixture()
        {
            _store = new Lazy<IDocumentStore>(TestingDocumentStore.Basic);

            if (GetType().GetTypeInfo().GetCustomAttribute<CollectionAttribute>(true) != null)
            {
                UseDefaultSchema();
            }
        }

        protected IDocumentStore theStore => _store.Value;

        protected void UseDefaultSchema()
        {
            _store = new Lazy<IDocumentStore>(TestingDocumentStore.DefaultSchema);
        }

        protected void StoreOptions(Action<StoreOptions> configure)
        {
            _store = new Lazy<IDocumentStore>(() => TestingDocumentStore.For(configure));
        }

        public virtual void Dispose()
        {
            if (_store.IsValueCreated)
            {
                _store.Value.Dispose();
            }
        }
    }
}
