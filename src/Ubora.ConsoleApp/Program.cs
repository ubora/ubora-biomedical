using System;
using Autofac;
using Ubora.Domain.Infrastructure;

namespace Ubora.ConsoleApp
{
    internal class Program
    {
        private static IContainer CompositionRoot(string connectionString)
        {
            var autofacContainerBuilder = new ContainerBuilder();
            autofacContainerBuilder.RegisterType<Application>();

            var domainModule = new DomainAutofacModule(connectionString, storageProvider: null);
            autofacContainerBuilder.RegisterModule(domainModule);

            return autofacContainerBuilder.Build();
        }

        static void Main(string[] args)
        {
            if (string.IsNullOrWhiteSpace(args[0]))
            {
                throw new InvalidOperationException("Connection string not set.");
            }

            CompositionRoot(connectionString: args[0])
                .Resolve<Application>()
                .Run();
        }
    }
}
