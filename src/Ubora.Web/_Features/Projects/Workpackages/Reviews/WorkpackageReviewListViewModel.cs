using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Web._Features._Shared;
using Ubora.Web.Authorization;

namespace Ubora.Web._Features.Projects.Workpackages.Reviews
{
    public class WorkpackageReviewListViewModel
    {
        public IEnumerable<WorkpackageReviewViewModel> Reviews { get; set; }
        public string SubmitForReviewUrl { get; set; }
        public string ReviewDecisionUrl { get; set; }
        public UiElementVisibility SubmitForReviewButton { get; set; }

        /// <remarks>
        /// Logic moved here to reduce duplication by making the method generic.
        /// Although it's very possible that the button visibility will be different for each work package in the future.
        /// </remarks>
        public static async Task<UiElementVisibility> GetSubmitButtonVisibility<T>(T workpackage, ClaimsPrincipal user, IAuthorizationService authorizationService)
            where T : Workpackage<T>
        {
            if (workpackage.HasReviewInProcess || workpackage.HasBeenAccepted)
            {
                return UiElementVisibility.HiddenCompletely();
            }

            var isAuthenticated = await authorizationService.AuthorizeAsync(user, Policies.CanSubmitWorkpackageForReview);
            if (!isAuthenticated.Succeeded)
            {
                return UiElementVisibility.HiddenWithMessage("You can not submit work package for review, because you are not the project leader.");
            }
            return UiElementVisibility.Visible();
        }
    }
}