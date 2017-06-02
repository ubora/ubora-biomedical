using Autofac;
using AutoMapper;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Web.Services;
using Ubora.Web._Features.Users.Manage;
using Ubora.Web._Features._Shared;

namespace Ubora.Web.Infrastructure
{
    public class WebAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthMessageSender>().As<IEmailSender>().As<ISmsSender>().InstancePerLifetimeScope();
            builder.RegisterType<ModelStateUpdater>().As<IModelStateUpdater>().InstancePerLifetimeScope();
            builder.RegisterType<ManageValidator>().As<IManageValidator>().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly).Where(t => t.IsNested && t.Name.EndsWith("Factory")).InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(ThisAssembly).AsClosedTypesOf(typeof(IQueryHandler<,>)).InstancePerLifetimeScope();
        }

        public void AddAutoMapperProfiles(IMapperConfigurationExpression cfg)
        {
            cfg.AddProfiles(ThisAssembly);
        }
    }
}
