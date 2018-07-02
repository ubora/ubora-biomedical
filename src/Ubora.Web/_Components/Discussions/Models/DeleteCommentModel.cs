using System;

namespace Ubora.Web._Components.Discussions.Models
{
    public class DeleteCommentModel
    {
        public Guid CommentId { get; set; }
        public string CommentText { get; set; }
        public string DeleteCommentActionPath { get; set; }
    }
}