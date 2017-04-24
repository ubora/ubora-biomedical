using Autofac;
using Marten;
using Ubora.Domain.Infrastructure;

namespace Ubora.Domain.Tests
{
    public abstract class IntegrationFixture : DocumentSessionIntegrationFixture
    {
        private IContainer _innerContainer;
        protected IComponentContext Container => _innerContainer ?? (_innerContainer = InitializeContainer());

        private IContainer InitializeContainer()
        {
            var builder = new ContainerBuilder();

            var module = new DomainAutofacModule(ConnectionSource.ConnectionString);
            builder.RegisterModule(module);

            // Override DocumentStore registration (last is used)
            builder.RegisterInstance((DocumentStore)theStore).SingleInstance();

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