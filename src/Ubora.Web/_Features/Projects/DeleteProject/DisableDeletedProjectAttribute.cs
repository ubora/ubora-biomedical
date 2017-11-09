using Microsoft.AspNetCore.Mvc.Filters;

namespace Ubora.Web._Features.Projects.DeleteProject
{
    public class DontRedirectIfProjectDeletedAttribute : ActionFilterAttribute
    {
    }
}