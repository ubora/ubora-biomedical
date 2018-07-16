using Autofac;
using Ubora.Domain.Infrastructure;

namespace Ubora.ConsoleApp
{
    class Program
    {
        static private IContainer CompositionRoot()
        {
            var connectionString = "server=localhost;Port=5400;userid=postgres;password=ubora;database=postgres";
            var autofacContainerBuilder = new ContainerBuilder();
            autofacContainerBuilder.RegisterType<Application>();
            var domainModule = new DomainAutofacModule(connectionString, null);
            autofacContainerBuilder.RegisterModule(domainModule);
            return autofacContainerBuilder.Build();
        }

        static void Main(string[] args)
        {
            CompositionRoot().Resolve<Application>().Run();
        }
    }
}
