using Autofac;
using Ubora.Web.Services;

namespace Ubora.Web.Infrastructure
{
    public class WebAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthMessageSender>().As<IEmailSender>().As<ISmsSender>().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
