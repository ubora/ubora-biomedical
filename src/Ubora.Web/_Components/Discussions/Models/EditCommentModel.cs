using System;

namespace Ubora.Web._Components.Discussions.Models
{
    public class EditCommentModel
    {
        public Guid CommentId { get; set; }
        public string CommentText { get; set; }
        public string EditCommentActionPath { get; set; }
    }
}