using Marten;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
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

    public class TestingDocumentStore : DocumentStore
    {
        public new static IDocumentStore For(Action<StoreOptions> configure)
        {
            var options = new StoreOptions();
            options.Connection(ConnectionSource.ConnectionString);

            options.NameDataLength = 100;

            configure(options);

            options.DatabaseSchemaName = "uboratest" + Guid.NewGuid().ToString("N").ToLower();


            var store = new TestingDocumentStore(options);
            //store.Advanced.Clean.CompletelyRemoveAll();

            return store;
        }


        public static IDocumentStore Basic()
        {
            return For(_ =>
            {
            });
        }

        public static IDocumentStore DefaultSchema()
        {
            var store = For(_ =>
            {
                _.DatabaseSchemaName = StoreOptions.DefaultDatabaseSchemaName;
            });
            return store;
        }

        private TestingDocumentStore(StoreOptions options) : base(options)
        {
        }

        public override void Dispose()
        {
            var schemaName = Advanced.Options.DatabaseSchemaName;

            if (schemaName != StoreOptions.DefaultDatabaseSchemaName)
            {
                var sql = $"DROP SCHEMA {schemaName} CASCADE;";
                using (var conn = Advanced.OpenConnection())
                {
                    conn.Execute(cmd => cmd.CommandText = sql);
                }
            }

            base.Dispose();
        }
    }
}
