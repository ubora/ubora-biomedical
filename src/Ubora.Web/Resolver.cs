using System;
using Microsoft.Extensions.DependencyInjection;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Web
{
    public class Resolver : IResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public Resolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T Resolve<T>()
        {
            return _serviceProvider.GetService<T>();
        }
    }
}