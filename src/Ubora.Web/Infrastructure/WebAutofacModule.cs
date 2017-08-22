using Autofac;
using AutoMapper;
using Ubora.Domain.Infrastructure.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Web.Services;
using Ubora.Web._Features.Feedback;
using Ubora.Web._Features.Users.Account;
using Ubora.Web._Features.Notifications._Base;
using Ubora.Web._Features._Shared.Tokens;

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

            builder.RegisterType<ActionContextAccessor>().As<IActionContextAccessor>().SingleInstance();
            builder.RegisterType<UrlHelperFactory>().As<IUrlHelperFactory>().SingleInstance();
            builder.Register(c =>
            {
                var actionContext = c.Resolve<IActionContextAccessor>().ActionContext;
                var factory = c.Resolve<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            })
            .As<IUrlHelper>().InstancePerLifetimeScope();

            builder.RegisterType<ApplicationUserManager>().As<IApplicationUserManager>().InstancePerLifetimeScope();
            builder.RegisterType<ApplicationSignInManager>().As<IApplicationSignInManager>().InstancePerLifetimeScope();

            builder.RegisterType<AuthMessageSender>().As<IPasswordRecoveryMessageSender>().As<IEmailConfirmationMessageSender>().InstancePerLifetimeScope();
            builder.RegisterType<ViewRender>().InstancePerLifetimeScope();
            builder.RegisterType<ImageServices.ImageStorageProvider>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(ThisAssembly).Where(t => t.IsNested && t.Name.EndsWith("Factory")).InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(ThisAssembly).AsClosedTypesOf(typeof(IQueryHandler<,>)).InstancePerLifetimeScope();

            builder.RegisterType<SendFeedbackCommand.Handler>().As<ICommandHandler<SendFeedbackCommand>>().InstancePerLifetimeScope();

            builder.RegisterType<NotificationViewModelFactoryMediator>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .AsClosedTypesOf(typeof(NotificationViewModelFactory<,>)).As<INotificationViewModelFactory>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TokenReplacerMediator>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .AssignableTo<ITokenReplacer>().As<ITokenReplacer>()
                .InstancePerLifetimeScope();
        }

        public void AddAutoMapperProfiles(IMapperConfigurationExpression cfg)
        {
            cfg.AddProfiles(ThisAssembly);
        }
    }
}
