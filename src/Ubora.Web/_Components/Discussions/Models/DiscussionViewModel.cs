using System.Collections.Generic;

namespace Ubora.Web._Components.Discussions.Models
{
    public class DiscussionViewModel
    {
        public IReadOnlyCollection<CommentViewModel> Comments { get; set; }
        public string EditCommentActionPath { get; set; }
        public string DeleteCommentActionPath { get; set; }
        public string AddCommentActionPath { get; set; }
        public bool HideAddComment { get; set; }
    }
}