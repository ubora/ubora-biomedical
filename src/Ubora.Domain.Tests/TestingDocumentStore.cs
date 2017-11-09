using System;
using Marten;
using Marten.Services;

namespace Ubora.Domain.Tests
{
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
            var schemaName = this.Options.DatabaseSchemaName;

            if (schemaName != StoreOptions.DefaultDatabaseSchemaName)
            {
                var sql = $"DROP SCHEMA IF EXISTS {schemaName} CASCADE;";
                using (var conn = this.Tenancy.Default.OpenConnection())
                {
                    conn.Execute(sql);
                    conn.Commit();
                }
            }

            base.Dispose();
        }
    }
}