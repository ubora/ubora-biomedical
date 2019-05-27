using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ubora.Domain.ClinicalNeeds;
using Ubora.Domain.Users;
using Microsoft.Extensions.DependencyInjection;
using Ubora.Web.Infrastructure.Extensions;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed._Shared;

namespace Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed
{
    [Route("clinical-needs/{clinicalNeedId}")]
    public abstract class AClinicalNeedController : ClinicalNeedsAreaController
    {
        public ClinicalNeed ClinicalNeed { get; private set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var idAsString = RouteData.Values["clinicalNeedId"] as string;

            if (Guid.TryParse(idAsString, out Guid clinicalNeedId))
            {
                ClinicalNeed = QueryProcessor.FindById<ClinicalNeed>(clinicalNeedId);
            }

            if (ClinicalNeed == null)
            {
                context.Result = new NotFoundResult();
                return;
            }

            var imageStorageProvider = HttpContext.RequestServices.GetService<ImageStorageProvider>();

            var indicator = QueryProcessor.FindById<UserProfile>(ClinicalNeed.IndicatorUserId);
            var quickInfo = QueryProcessor.FindById<ClinicalNeedQuickInfo>(ClinicalNeed.Id);

            var urlTemplateParts = context.ActionDescriptor.AttributeRouteInfo.Template.Split("/");

            ViewData["LayoutViewModel"] = new LayoutViewModel
            {
                ActiveTab = GetCurrentTab(urlTemplateParts),
                IndicatorUserId = indicator.UserId,
                IndicatorFullName = indicator.FullName,
                IndicatedAt = ClinicalNeed.IndicatedAt,
                IndicatorProfilePictureUrl = imageStorageProvider.GetDefaultOrBlobUrl(indicator),
                NumberOfRelatedProjects = quickInfo.NumberOfRelatedProjects,
                NumberOfComments = quickInfo.NumberOfComments,
                ClinicalNeedTitle = ClinicalNeed.Title
            };
            ViewData[nameof(PageTitle)] = ClinicalNeed.Title;
        }

        private ActiveTabOfClinicalNeed GetCurrentTab(string[] urlTemplateParts)
        {
            var currentTabString = urlTemplateParts
                .SkipWhile(urlPart => urlPart != "{clinicalNeedId}")
                .ElementAtOrDefault(1);

            switch (currentTabString)
            {
                case "comments":
                    return ActiveTabOfClinicalNeed.Comments;
                case "related-projects":
                    return ActiveTabOfClinicalNeed.RelatedProjects;
                default:
                    return ActiveTabOfClinicalNeed.Description;
            }
        }
    }
}