using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Web.Services;

namespace Ubora.Web._Features.Projects
{
    public abstract class ProjectController : UboraController
    {
        protected ProjectController(ICommandQueryProcessor processor) : base(processor)
        {
        }

        protected virtual UserInfo UserInfo => User.GetInfo();
    }
}