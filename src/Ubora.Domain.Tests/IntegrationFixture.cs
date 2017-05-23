using Autofac;
using Marten;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Marten;

namespace Ubora.Domain.Tests
{
    public abstract class IntegrationFixture : DocumentSessionIntegrationFixture
    {
        private IContainer _innerContainer;
        protected IComponentContext Container => _innerContainer ?? (_innerContainer = InitializeContainer());

        private ICommandQueryProcessor _processor;
        protected ICommandQueryProcessor Processor => _processor ?? (_processor = Container.Resolve<ICommandQueryProcessor>());

        private readonly DomainAutofacModule _domainAutofacModule;

        protected IntegrationFixture()
        {
            _domainAutofacModule = new DomainAutofacModule(ConnectionSource.ConnectionString);
            var eventTypes = _domainAutofacModule.FindDomainEventConcreteTypes();
            StoreOptions(new UboraStoreOptions().Configuration(eventTypes));
        }

        private IContainer InitializeContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(_domainAutofacModule);

            // Override DocumentStore/Session registration (last is used)
            builder.Register(_ => theStore).SingleInstance();
            builder.Register(_ => Session).As<IDocumentSession>();

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