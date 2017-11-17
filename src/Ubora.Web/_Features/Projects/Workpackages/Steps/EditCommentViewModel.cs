using System;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    public class EditCommentViewModel
    {
        public Guid Id { get; set; }
        public Guid CandidateId { get; set; }
        public string CommentText { get; set; }
    }
}