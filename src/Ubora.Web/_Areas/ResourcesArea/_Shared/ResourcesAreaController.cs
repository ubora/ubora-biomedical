using Microsoft.AspNetCore.Mvc;
using Ubora.Web._Features;

namespace Ubora.Web._Areas.ResourcesArea._Shared
{
    [Area(Areas.ResourcesArea)]
    [Route("resources")]
    public abstract class ResourcesAreaController : UboraController
    {
    }
}
