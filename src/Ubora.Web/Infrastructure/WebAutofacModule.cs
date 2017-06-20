using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Web.Services;

namespace Ubora.Web.Infrastructure
{
    public class WebAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EmailSender>().As<IEmailSender>().InstancePerLifetimeScope();
            builder.RegisterType<SmsSender>().As<ISmsSender>().InstancePerLifetimeScope();
            builder.RegisterType<AuthMessageSender>().As<IAuthMessageSender>().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly).Where(t => t.IsNested && t.Name.EndsWith("Factory")).InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(ThisAssembly).AsClosedTypesOf(typeof(IQueryHandler<,>)).InstancePerLifetimeScope();
        }

        public void AddAutoMapperProfiles(IMapperConfigurationExpression cfg)
        {
            cfg.AddProfiles(ThisAssembly);
        }
    }
}
