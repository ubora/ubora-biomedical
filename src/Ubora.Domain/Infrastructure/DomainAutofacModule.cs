using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Marten;
using Marten.Events;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Infrastructure.Marten;
using Ubora.Domain.Infrastructure.Queries;
using Module = Autofac.Module;

namespace Ubora.Domain.Infrastructure
{
    public class DomainAutofacModule : Module
    {
        private readonly string _connectionString;

        public DomainAutofacModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            if (ShouldInitializeAndRegisterDocumentStoreOnLoad)
            {
                var options = new StoreOptions();
                options.Connection(_connectionString);

                options.NameDataLength = 100;

                var eventTypes = FindDomainEventConcreteTypes();
                var configureAction = new UboraStoreOptions().Configuration(eventTypes);

                configureAction.Invoke(options);

                builder.RegisterInstance(new DocumentStore(options)).SingleInstance();
            }

            builder.Register(x => x.Resolve<DocumentStore>().OpenSession()).As<IDocumentSession>().As<IQuerySession>().InstancePerLifetimeScope();
            builder.Register(x => x.Resolve<IDocumentSession>().Events).As<IEventStore>().InstancePerLifetimeScope();

            builder.RegisterType<EventStreamQuery>().As<IEventStreamQuery>().InstancePerLifetimeScope();
            builder.RegisterType<CommandQueryProcessor>().As<ICommandProcessor>().As<IQueryProcessor>().As<ICommandQueryProcessor>().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly).AsClosedTypesOf(typeof(ICommandHandler<>)).InstancePerLifetimeScope();
        }

        public IEnumerable<Type> FindDomainEventConcreteTypes()
        {
            var eventBaseType = typeof(UboraEvent);

            var eventTypes = ThisAssembly
                .GetTypes()
                .Where(type => eventBaseType.IsAssignableFrom(type) && !type.GetTypeInfo().IsAbstract);

            return eventTypes;
        }

        // Static helper for tests
        internal static bool ShouldInitializeAndRegisterDocumentStoreOnLoad { get; set; } = true;
    }
}
