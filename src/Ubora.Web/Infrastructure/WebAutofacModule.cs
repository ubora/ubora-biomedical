using Autofac;
using AutoMapper;
using Ubora.Domain.Infrastructure.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.Workpackages.Events;
using Ubora.Domain.Questionnaires.ApplicableRegulations;
using Ubora.Domain.Questionnaires.DeviceClassifications;
using Ubora.Web.Infrastructure.PreMailers;
using Ubora.Web.Services;
using Ubora.Web._Features.Feedback;
using Ubora.Web._Features.Users.Account;
using Ubora.Web._Features.Notifications._Base;
using Ubora.Web._Features._Shared.Tokens;
using Ubora.Web.Infrastructure.Storage;
using Ubora.Web._Features.Projects.ApplicableRegulations;
using Ubora.Web._Features.Projects.DeviceClassifications;
using Ubora.Web._Features.Projects.History._Base;
using Ubora.Web._Features.Projects.Workpackages.Steps;

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
                builder.RegisterType<SmtpEmailSender>().As<EmailSender>()
                    .InstancePerLifetimeScope();
            }
            else
            {
                builder.RegisterType<SpecifiedPickupDirectoryEmailSender>().As<EmailSender>()
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

            builder.RegisterType<ApplicationUserEmailMessageSender>()
                .As<IPasswordRecoveryMessageSender>().As<IEmailConfirmationMessageSender>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ViewRender>().InstancePerLifetimeScope();
            builder.RegisterType<ImageServices.ImageStorageProvider>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.IsNested && t.Name.EndsWith("Factory"))
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .AsClosedTypesOf(typeof(IQueryHandler<,>))
                .InstancePerLifetimeScope();

            builder.RegisterType<SendFeedbackCommand.Handler>().As<ICommandHandler<SendFeedbackCommand>>().InstancePerLifetimeScope();

            builder.RegisterType<NotificationViewModelFactoryMediator>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .AsClosedTypesOf(typeof(NotificationViewModelFactory<,>)).As<INotificationViewModelFactory>()
                .InstancePerLifetimeScope();

            builder.RegisterType<EventNotificationViewModel.Factory>().As<INotificationViewModelFactory>()
                .InstancePerLifetimeScope();

            builder.RegisterType<EventViewModelFactoryMediator>().As<IEventViewModelFactoryMediator>().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .AsClosedTypesOf(typeof(EventViewModelFactory<,>)).As<IEventViewModelFactory>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TokenReplacerMediator>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .AssignableTo<ITokenReplacer>().As<ITokenReplacer>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UboraStorageProvider>().As<IUboraStorageProvider>().InstancePerLifetimeScope();

            builder.RegisterType<PreMailerFactory>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<UserAndEnvironmentInformationViewModel.Mapper>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<HealthTechnologySpecificationsViewModel.Mapper>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterType<DeviceClassificationIndexViewModel.QuestionnaireListItemProjection>()
                .As<IProjection<DeviceClassificationAggregate, DeviceClassificationIndexViewModel.QuestionnaireListItem>>()
                .SingleInstance();

            builder.RegisterType<QuestionnaireIndexViewModel.QuestionnaireListItemProjection>()
                .As<IProjection<ApplicableRegulationsQuestionnaireAggregate, QuestionnaireIndexViewModel.QuestionnaireListItem>>()
                .SingleInstance();

            builder.RegisterType<UboraEventHandlerInvoker>().AsSelf().SingleInstance();
        }

        public void AddAutoMapperProfiles(IMapperConfigurationExpression cfg)
        {
            cfg.AddProfiles(ThisAssembly);
        }
    }
}
