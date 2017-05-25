using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;

namespace Ubora.Web.Tests.Authorization
{
    public class EmptyInitializedActionContext : ActionContext
    {
        public EmptyInitializedActionContext()
        {
            RouteData = new RouteData();
            ActionDescriptor = new ActionDescriptor();
            HttpContext = new DefaultHttpContext();
        }
    }
}