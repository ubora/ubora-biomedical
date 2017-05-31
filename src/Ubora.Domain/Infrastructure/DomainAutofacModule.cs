using System;
using System.IO;
using Autofac;
using Marten;
using Marten.Events;
using TwentyTwenty.Storage;
using TwentyTwenty.Storage.Amazon;
using TwentyTwenty.Storage.Local;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Marten;
using Ubora.Domain.Infrastructure.Queries;
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
            var options = new StoreOptions();
            options.Connection(_connectionString);

            options.NameDataLength = 100;

            Action<StoreOptions> configuration = new UboraStoreOptions().Configuration();

            configuration.Invoke(options);

            var amazonProviderOptions = new AmazonProviderOptions()
            {
                Bucket = Environment.GetEnvironmentVariable("amazon_storage_bucket"),
                PublicKey = Environment.GetEnvironmentVariable("amazon_storage_publickey"),
                SecretKey = Environment.GetEnvironmentVariable("amazon_storage_secretkey")
            };

            var basePath = Path.GetFullPath("storages");

            builder.RegisterInstance(new DocumentStore(options)).SingleInstance();
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
    }
}
