using System;
using Microsoft.Extensions.DependencyInjection;
using Ubora.Domain;
using Ubora.Domain.Commands;

namespace Ubora.Web
{
    public class DomainIocModule : IocModule
    {
        private readonly IServiceCollection _serviceCollection;

        public DomainIocModule(IServiceCollection serviceCollection, string connectionString) : base(connectionString)
        {
            _serviceCollection = serviceCollection;
        }

        public override void RegisterInScope<T, TImpl>()
        {
            _serviceCollection.AddScoped<T, TImpl>();
        }

        public override void RegisterInstanceInScope<T>(Func<T> factory)
        {
            _serviceCollection.AddScoped<T>(serviceProvider => factory.Invoke());
        }

        public override void RegisterInstanceInScope<T>(Func<IResolver, T> factory)
        {
            _serviceCollection.AddScoped<T>(serviceProvider =>
            {
                var resolver = new Resolver(serviceProvider);
                return factory.Invoke(resolver);
            });
        }
    }
}