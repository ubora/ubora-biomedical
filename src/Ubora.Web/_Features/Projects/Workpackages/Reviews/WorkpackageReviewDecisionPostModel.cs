namespace Ubora.Web._Features.Projects.Workpackages.Reviews
{
    public class WorkpackageReviewDecisionPostModel
    {
        public bool IsAccept { get; set; }
        public bool IsReject { get; set; }
        public string ConcludingComment { get; set; }
        public string RejectUrl { get; set; }
        public string AcceptUrl { get; set; }
    }
}