using System;
using Autofac;
using AutoMapper;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Web.Services;

namespace Ubora.Web.Infrastructure
{
    public class WebAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var areSmtpEnvironmentVariablesSet =
                Environment.GetEnvironmentVariable("smtp_hostname") != null &&
                Environment.GetEnvironmentVariable("smtp_port") != null &&
                Environment.GetEnvironmentVariable("smtp_username") != null &&
                Environment.GetEnvironmentVariable("smtp_password") != null;

            if (areSmtpEnvironmentVariablesSet)
            {
                builder.RegisterType<SmtpEmailSender>().As<IEmailSender>()
                    .InstancePerLifetimeScope();
            }
            else
            {
                builder.RegisterType<SpecifiedPickupDirectoryEmailSender>().As<IEmailSender>()
                    .InstancePerLifetimeScope();
            }
            
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
