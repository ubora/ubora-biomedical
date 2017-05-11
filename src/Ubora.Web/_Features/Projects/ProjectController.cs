using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Web.Services;

namespace Ubora.Web._Features.Projects
{
    public abstract class ProjectController : Controller
    {
        protected virtual UserInfo UserInfo => User.GetInfo();
    }
}
