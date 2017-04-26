using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ubora.Web.Areas.Projects.Controllers.Shared
{
    [Authorize]
    [Area("Projects")]
    public abstract class ProjectsController : Controller
    {
    }
}
