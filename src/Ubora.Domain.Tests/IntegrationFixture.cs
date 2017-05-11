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

        protected IntegrationFixture()
        {
            StoreOptions(new UboraStoreOptions().Configuration());
        }

        private IContainer InitializeContainer()
        {
            var builder = new ContainerBuilder();

            var module = new DomainAutofacModule(ConnectionSource.ConnectionString);
            builder.RegisterModule(module);

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