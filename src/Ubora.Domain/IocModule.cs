using System;
using System.Collections.Generic;
using System.Text;
using Marten;
using Ubora.Domain.Commands;
using Ubora.Domain.Projects;
using Ubora.Domain.Queries;

namespace Ubora.Domain
{
    public abstract class IocModule
    {
        private readonly string _connectionString;
        private static DocumentStore _martenSingleton;

        public IocModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        public abstract void RegisterInScope<T, TImpl>() where T : class where TImpl : class, T;
        public abstract void RegisterInstanceInScope<T>(Func<T> factory) where T : class;

        public void Load()
        {
            var options = new StoreOptions();
            options.Connection(_connectionString);

            options.NameDataLength = 100;

            Action<StoreOptions> configuration = new UboraStoreOptions().Configuration();

            configuration.Invoke(options);

            _martenSingleton = new DocumentStore(options);
            RegisterInstanceInScope<IDocumentSession>(() => _martenSingleton.OpenSession());
            RegisterInstanceInScope<IQuerySession>(() => _martenSingleton.QuerySession());

            RegisterInScope<IQuery, Query>();
            RegisterInScope<ICommandProcessor, CommandProcessor>();
            RegisterInScope<ICommandHandler<CreateProjectCommand>, CreateProjectCommandHandler>();
        }
    }
}
