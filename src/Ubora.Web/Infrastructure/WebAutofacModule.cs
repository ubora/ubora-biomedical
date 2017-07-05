using Autofac;
using AutoMapper;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Web._Features.Projects.DeviceClassification.Services;
using Ubora.Web._Features.Notifications.Factory;
using Ubora.Web.Services;

namespace Ubora.Web.Infrastructure
{
    public class WebAutofacModule : Module
    {
        private readonly bool _useSpecifiedPickupDirectory;

        public WebAutofacModule(bool useSpecifiedPickupDirectory)
        {
            _useSpecifiedPickupDirectory = useSpecifiedPickupDirectory;
        }

        protected override void Load(ContainerBuilder builder)
        {
            if (!_useSpecifiedPickupDirectory)
            {
                builder.RegisterType<SmtpEmailSender>().As<IEmailSender>()
                    .InstancePerLifetimeScope();
            }
            else
            {
                builder.RegisterType<SpecifiedPickupDirectoryEmailSender>().As<IEmailSender>()
                    .InstancePerLifetimeScope();
            }

            builder.RegisterType<AuthMessageSender>().As<IAuthMessageSender>().InstancePerLifetimeScope();
            builder.RegisterType<NotificationViewModelFactory>().As<INotificationViewModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<ImageResizer>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(ThisAssembly).Where(t => t.IsNested && t.Name.EndsWith("Factory")).InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(ThisAssembly).AsClosedTypesOf(typeof(IQueryHandler<,>)).InstancePerLifetimeScope();
        }

        public void AddAutoMapperProfiles(IMapperConfigurationExpression cfg)
        {
            cfg.AddProfiles(ThisAssembly);
        }
    }
}
