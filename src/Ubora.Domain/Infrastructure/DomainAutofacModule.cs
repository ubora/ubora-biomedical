using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using Autofac;
using Marten;
using Marten.Events;
using TwentyTwenty.Storage;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Infrastructure.Marten;
using Ubora.Domain.Infrastructure.Queries;
using Module = Autofac.Module;
using Ubora.Domain.Projects.DeviceClassification;

namespace Ubora.Domain.Infrastructure
{
    public class DomainAutofacModule : Module
    {
        private readonly string _connectionString;
        private readonly IStorageProvider _storageProvider;

        public DomainAutofacModule(string connectionString, IStorageProvider storageProvider)
        {
            _connectionString = connectionString;
            _storageProvider = storageProvider;
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

                builder.RegisterInstance(new DocumentStore(options)).As<IDocumentStore>().SingleInstance();
            }

           
            builder.Register(x => x.Resolve<IDocumentStore>().OpenSession()).As<IDocumentSession>().As<IQuerySession>().InstancePerLifetimeScope();
            builder.Register(x => x.Resolve<IDocumentSession>().Events).As<IEventStore>().InstancePerLifetimeScope();

            builder.RegisterType<EventStreamQuery>().As<IEventStreamQuery>().InstancePerLifetimeScope();
            builder.RegisterType<DeviceClassificationProvider>().As<IDeviceClassificationProvider>().InstancePerLifetimeScope();
            builder.RegisterType<CommandQueryProcessor>().As<ICommandProcessor>().As<IQueryProcessor>().As<ICommandQueryProcessor>().InstancePerLifetimeScope();
            
            // Storage abstraction
            builder.Register(x => _storageProvider)
                .As<IStorageProvider>()
                .SingleInstance();
       
            builder.RegisterAssemblyTypes(ThisAssembly).AsClosedTypesOf(typeof(ICommandHandler<>)).InstancePerLifetimeScope();
            builder.RegisterType<DeviceClassification>().As<IDeviceClassification>().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly).AsClosedTypesOf(typeof(IQueryHandler<,>)).InstancePerLifetimeScope();
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
