﻿using Autofac;
using AutoMapper;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Web._Features.Projects.DeviceClassification.Services;
using Ubora.Web._Features.Notifications.Factory;
using Ubora.Web.Services;

namespace Ubora.Web.Infrastructure
{
    public class WebAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthMessageSender>().As<IEmailSender>().As<ISmsSender>().InstancePerLifetimeScope();
            builder.RegisterType<NotificationViewModelFactory>().As<INotificationViewModelFactory>().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly).Where(t => t.IsNested && t.Name.EndsWith("Factory")).InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(ThisAssembly).AsClosedTypesOf(typeof(IQueryHandler<,>)).InstancePerLifetimeScope();
        }

        public void AddAutoMapperProfiles(IMapperConfigurationExpression cfg)
        {
            cfg.AddProfiles(ThisAssembly);
        }
    }
}
