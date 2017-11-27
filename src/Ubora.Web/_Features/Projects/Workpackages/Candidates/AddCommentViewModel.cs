using System;
using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Features.Projects.Workpackages.Candidates
{
    public class AddCommentViewModel
    {
        public Guid CandidateId { get; set; }
        [Required]
        public string CommentText { get; set; }
    }
}
