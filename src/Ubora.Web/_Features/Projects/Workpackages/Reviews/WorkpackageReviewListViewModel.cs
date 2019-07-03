using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Web._Features._Shared;
using Ubora.Web.Authorization;
using Ubora.Domain.Projects;

namespace Ubora.Web._Features.Projects.Workpackages.Reviews
{
    public class WorkpackageReviewListViewModel
    {
        public WorkpackageReviewListViewModel(
            IEnumerable<WorkpackageReviewViewModel> reviews,
            string submitForReviewUrl,
            string reviewDecisionUrl,
            UiElementVisibility submitForReviewButton,
            UiElementVisibility requestMentoringButton,
            WorkpackageReviewViewModel latestReview)
        {
            Reviews = reviews;
            SubmitForReviewUrl = submitForReviewUrl;
            ReviewDecisionUrl = reviewDecisionUrl;
            SubmitForReviewButton = submitForReviewButton;
            RequestMentoringButton = requestMentoringButton;
            LatestReview = latestReview;
        }

        public IEnumerable<WorkpackageReviewViewModel> Reviews { get; }
        public string SubmitForReviewUrl { get; }
        public string ReviewDecisionUrl { get; }
        public UiElementVisibility SubmitForReviewButton { get; }
        public UiElementVisibility RequestMentoringButton { get; }
        public WorkpackageReviewViewModel LatestReview { get; }

        public bool IsAnyReviewInProcess => Reviews.Any(x => x.Status == WorkpackageReviewStatus.InProcess);

        public async Task<bool> IsWriteReviewButtonVisible(ClaimsPrincipal user, IAuthorizationService authorizationService)
        {
            var isAuthorizedToWriteReview = await authorizationService.IsAuthorizedAsync(user, Policies.CanReviewProjectWorkpackages);
            return isAuthorizedToWriteReview && IsAnyReviewInProcess;
        }

        public async Task<bool> IsReopenWp1ButtonVisible(ClaimsPrincipal user, IAuthorizationService authorizationService)
        {
            var isAuthorizedToWriteReview = await authorizationService.IsAuthorizedAsync(user, Policies.CanReviewProjectWorkpackages);
            return isAuthorizedToWriteReview && LatestReview?.Status == WorkpackageReviewStatus.Accepted;
        }

        /// <remarks>
        /// Logic moved here to reduce duplication by making the method generic.
        /// Although it's very possible that the button visibility will be different for each work package in the future.
        /// </remarks>
        public static async Task<UiElementVisibility> GetSubmitButtonVisibility(Project project, WorkpackageOne workpackage, ClaimsPrincipal user, IAuthorizationService authorizationService)
        {
            if (workpackage.HasReviewInProcess || workpackage.HasBeenAccepted)
            {
                return UiElementVisibility.HiddenCompletely();
            }

            var isAuthorized = await authorizationService.IsAuthorizedAsync(user, Policies.CanSubmitWorkpackageForReview);
            if (!isAuthorized)
            {
                return UiElementVisibility.HiddenWithMessage("You can not submit work package for review, because you are not the project leader.");
            }

            if (!project.Members.Any(m => m.IsMentor))
            {
                return UiElementVisibility.HiddenCompletely();
            }

            return UiElementVisibility.Visible();
        }

        public static async Task<UiElementVisibility> GetRequestMentoringButtonVisibility(Project project, WorkpackageOne workpackage, ClaimsPrincipal user, IAuthorizationService authorizationService)
        {
            var isAuthorized = await authorizationService.IsAuthorizedAsync(user, Policies.CanSubmitWorkpackageForReview);

            if (workpackage.HasReviewInProcess || workpackage.HasBeenAccepted || !isAuthorized)
            {
                return UiElementVisibility.HiddenCompletely();
            }

            if (!project.Members.Any(m => m.IsMentor))
            {
                if (!workpackage.HasBeenRequestedMentoring)
                {
                    return UiElementVisibility.Visible();
                }
                return UiElementVisibility.HiddenWithMessage("You have requested a mentor. Please wait until one is appointed to your project.");
            }

            return UiElementVisibility.HiddenCompletely();
        }
    }
}