using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Marten;
using Marten.Events;
using TwentyTwenty.Storage;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Infrastructure.Marten;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Notifications;
using Ubora.Domain.Projects.Workpackages.Events;
using Ubora.Domain.Queries;
using Ubora.Domain.Questionnaires.DeviceClassifications;

namespace Ubora.Domain.Infrastructure
{
    public class DomainAutofacModule : Autofac.Module
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

                var eventTypes = FindDomainEventConcreteTypes();
                var notificationTypes = FindDomainNotificationConcreteTypes();

                new UboraStoreOptionsConfigurer()
                    .CreateConfigureAction(eventTypes, notificationTypes, AutoCreate.CreateOnly)
                    .Invoke(options);

                var store = new DocumentStore(options);

                builder.RegisterInstance(store).As<IDocumentStore>().SingleInstance();
            }

            builder.RegisterType<DomainMigrator>().AsSelf().SingleInstance();

            builder.Register(x => x.Resolve<IDocumentStore>().OpenSession())
                .As<IDocumentSession>().As<IQuerySession>()
                .InstancePerLifetimeScope();

            builder.Register(x => x.Resolve<IDocumentSession>().Events)
                .As<IEventStore>()
                .InstancePerLifetimeScope();

            builder.RegisterType<EventStreamQuery>().As<IEventStreamQuery>().InstancePerLifetimeScope();

            builder.RegisterType<CommandQueryProcessor>()
                .As<ICommandProcessor>().As<IQueryProcessor>().As<ICommandQueryProcessor>()
                .InstancePerLifetimeScope();

            builder.Register(x => _storageProvider)
                .As<IStorageProvider>()
                .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .AsClosedTypesOf(typeof(ICommandHandler<>))
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .AsClosedTypesOf(typeof(IQueryHandler<,>))
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .AsClosedTypesOf(typeof(UboraEventHandler<>))
                .InstancePerLifetimeScope();

            builder.RegisterType<CountQuery<INotification>.Handler>()
                .As<IQueryHandler<CountQuery<INotification>, int>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DeviceClassificationQuestionnaireTreeFactory>().AsSelf().SingleInstance();

            builder.RegisterBuildCallback(container =>
            {
                AddUboraEventHandlerInvokerToDocumentStoreListenersIfNecessary(container);
            });
        }

        public static IEnumerable<Type> FindDomainEventConcreteTypes()
        {
            return typeof(UboraEvent).Assembly
                .GetTypes()
                .Where(type => typeof(UboraEvent).IsAssignableFrom(type) && !type.IsAbstract);
        }

        public static IEnumerable<MappedType> FindDomainNotificationConcreteTypes() 
        {
            return typeof(INotification).Assembly
                .GetTypes()
                .Where(type => typeof(INotification).IsAssignableFrom(type) && !type.IsAbstract)
                .Select(type => new MappedType(type));
        }

        private static void AddUboraEventHandlerInvokerToDocumentStoreListenersIfNecessary(IContainer container)
        {
            var documentStore = (DocumentStore) container.Resolve<IDocumentStore>();

            var isAlreadyRegistered = documentStore.Options.Listeners.OfType<UboraEventHandlerInvoker>().Any();
            if (isAlreadyRegistered)
            {
                // It is possible to build Autofac's Container multiple times but we should definitely not add the listener multiple times.
                return;
            }

            var serviceLocator = container.Resolve<IComponentContext>();
            var uboraEventHandlerInvoker = new UboraEventHandlerInvoker(serviceLocator);

            documentStore.Options.Listeners.Add(uboraEventHandlerInvoker);
        }

        /// <summary>
        /// Static helper for tests
        /// </summary>
        internal static bool ShouldInitializeAndRegisterDocumentStoreOnLoad { get; set; } = true;
    }
}