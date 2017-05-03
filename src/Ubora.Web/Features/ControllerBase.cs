using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Web.Services;

namespace Ubora.Web.Features
{
    public abstract class ControllerBase : Controller
    {
        protected virtual UserInfo UserInfo => new UserInfo(User.GetUserId(), User.GetFullName());
    }
}
