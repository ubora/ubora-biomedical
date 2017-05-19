using Autofac;
using AutoMapper;
using Ubora.Web._Features.Projects.DeviceClassification.Services;
using Ubora.Web.Services;

namespace Ubora.Web.Infrastructure
{
    public class WebAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthMessageSender>().As<IEmailSender>().As<ISmsSender>().InstancePerLifetimeScope();
            builder.RegisterType<DeviceClassification>().As<IDeviceClassification>().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly).Where(t => t.IsNested && t.Name.EndsWith("Factory")).InstancePerLifetimeScope();
        }

        public void AddAutoMapperProfiles(IMapperConfigurationExpression cfg)
        {
            cfg.AddProfiles(ThisAssembly);
        }
    }
}
