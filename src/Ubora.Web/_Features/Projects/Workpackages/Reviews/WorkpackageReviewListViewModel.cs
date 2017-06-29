using System.Collections.Generic;

namespace Ubora.Web._Features.Projects.Workpackages.Reviews
{
    public class WorkpackageReviewListViewModel
    {
        public IEnumerable<WorkpackageReviewViewModel> Reviews { get; set; }
        public string SubmitForReviewUrl { get; set; }
        public string ReviewDecisionUrl { get; set; }
    }
}