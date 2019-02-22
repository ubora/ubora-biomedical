using Microsoft.AspNetCore.Mvc;
using Ubora.Web._Features;

namespace Ubora.Web._Areas.ClinicalNeedsArea
{
    [Area(Areas.ClinicalNeedsArea)]
    [Route("clinical-needs")]
    public abstract class ClinicalNeedsAreaController : UboraController
    {
    }
}
