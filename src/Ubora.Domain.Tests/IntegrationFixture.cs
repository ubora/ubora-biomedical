using Autofac;
using Marten;
using Moq;
using TwentyTwenty.Storage;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Marten;

namespace Ubora.Domain.Tests
{
    public abstract class IntegrationFixture : DocumentSessionIntegrationFixture
    {
        private IContainer _innerContainer;
        public IComponentContext Container => _innerContainer ?? (_innerContainer = InitializeContainer());

        private ICommandQueryProcessor _processor;
        public  ICommandQueryProcessor Processor => _processor ?? (_processor = Container.Resolve<ICommandQueryProcessor>());

        private readonly DomainAutofacModule _domainAutofacModule;

        static IntegrationFixture()
        {
            DomainAutofacModule.ShouldInitializeAndRegisterDocumentStoreOnLoad = false;
        }

        protected IntegrationFixture()
        {
            _domainAutofacModule = new DomainAutofacModule(ConnectionSource.ConnectionString, Mock.Of<IStorageProvider>());
            var eventTypes = _domainAutofacModule.FindDomainEventConcreteTypes();
            StoreOptions(new UboraStoreOptions().Configuration(eventTypes));
        }

        private IContainer InitializeContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(_domainAutofacModule);

            // Register Marten DocumentStore/Session
            builder.Register(_ => (TestingDocumentStore)theStore).As<DocumentStore>().As<IDocumentStore>().SingleInstance();
            builder.Register(_ => Session).As<IDocumentSession>();

            var storageProviderMock = new Mock<IStorageProvider>().Object;
            builder.RegisterInstance(storageProviderMock).As<IStorageProvider>();

            RegisterAdditional(builder);

            var container = builder.Build();
            return container;
        }

        // Extension point for test-specific registrations (e.g. mocks)
        protected virtual void RegisterAdditional(ContainerBuilder builder)
        {
        }

        public override void Dispose()
        {
            _innerContainer?.Dispose();

            base.Dispose();
        }
    }
}