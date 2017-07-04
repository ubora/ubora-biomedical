using System.Collections.Generic;
using Ubora.Web._Features._Shared;

namespace Ubora.Web._Features.Projects.Workpackages.Reviews
{
    public class WorkpackageReviewListViewModel
    {
        public IEnumerable<WorkpackageReviewViewModel> Reviews { get; set; }
        public string SubmitForReviewUrl { get; set; }
        public string ReviewDecisionUrl { get; set; }
        public UiElementVisibility SubmitForReviewButton { get; set; }
    }
}