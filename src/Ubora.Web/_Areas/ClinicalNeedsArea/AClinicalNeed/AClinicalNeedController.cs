using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ubora.Domain.ClinicalNeeds;

namespace Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed
{
    [Route("clinical-needs/{clinicalNeedId}")]
    public abstract class AClinicalNeedController : ClinicalNeedsAreaController
    {
        public ClinicalNeed ClinicalNeed { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var idAsString = RouteData.Values["clinicalNeedId"] as string;

            if (Guid.TryParse(idAsString, out Guid clinicalNeedId))
            {
                ClinicalNeed = QueryProcessor.FindById<ClinicalNeed>(clinicalNeedId);
            }

            if (ClinicalNeed != null)
            {
                ViewData["Title"] = ClinicalNeed.Title;
            }
            else
            {
                context.Result = new NotFoundResult();
            }
        }
    }
}