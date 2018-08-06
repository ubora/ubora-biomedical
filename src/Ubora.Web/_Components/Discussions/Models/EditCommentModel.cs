using System;
using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Components.Discussions.Models
{
    public class EditCommentModel
    {
        public Guid CommentId { get; set; }
        [Required]
        public string CommentText { get; set; }
        public string EditCommentActionPath { get; set; }
    }
}