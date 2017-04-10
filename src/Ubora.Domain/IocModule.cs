using System;
using Marten;
using Marten.Events;
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
        public abstract void RegisterInstanceInScope<T>(Func<IResolver, T> factory) where T : class;

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
            RegisterInstanceInScope<IEventStore>(resolver => resolver.Resolve<IDocumentSession>().Events);

            RegisterInScope<IQuery, Query>();
            RegisterInScope<IEventStreamQuery, EventStreamQuery>();
            RegisterInScope<ICommandBus, CommandBus>();
            RegisterInScope<ICommandHandler<CreateProjectCommand>, CreateProjectCommandHandler>();
        }
    }
}
