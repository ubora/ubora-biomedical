using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using Autofac;
using Marten;
using Marten.Events;
using TwentyTwenty.Storage;
using TwentyTwenty.Storage.Amazon;
using TwentyTwenty.Storage.Local;
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

            var amazonProviderOptions = new AmazonProviderOptions()
            {
                Bucket = Environment.GetEnvironmentVariable("amazon_storage_bucket"),
                PublicKey = Environment.GetEnvironmentVariable("amazon_storage_publickey"),
                SecretKey = Environment.GetEnvironmentVariable("amazon_storage_secretkey"),
                ServiceUrl = "https://s3-eu-west-1.amazonaws.com"
            };

            var basePath = Path.GetFullPath("wwwroot/images/storages");

            builder.Register(x => x.Resolve<DocumentStore>().OpenSession()).As<IDocumentSession>().As<IQuerySession>().InstancePerLifetimeScope();
            builder.Register(x => x.Resolve<IDocumentSession>().Events).As<IEventStore>().InstancePerLifetimeScope();

            builder.RegisterType<EventStreamQuery>().As<IEventStreamQuery>().InstancePerLifetimeScope();
            builder.RegisterType<CommandQueryProcessor>().As<ICommandProcessor>().As<IQueryProcessor>().As<ICommandQueryProcessor>().InstancePerLifetimeScope();

            if (Environment.GetEnvironmentVariable("amazon_storage_bucket") != null || Environment.GetEnvironmentVariable("amazon_storage_publickey")
                != null || Environment.GetEnvironmentVariable("amazon_storage_secretkey") != null)
            {
                builder.RegisterInstance(new AmazonStorageProvider(amazonProviderOptions)).SingleInstance();
                builder.Register(x => x.Resolve<AmazonStorageProvider>())
                    .As<IStorageProvider>()
                    .InstancePerLifetimeScope();
            }
            else
            {
                builder.RegisterInstance(new LocalStorageProvider(basePath)).SingleInstance();
                builder.Register(x => x.Resolve<LocalStorageProvider>()).As<IStorageProvider>().InstancePerLifetimeScope();
            }

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
