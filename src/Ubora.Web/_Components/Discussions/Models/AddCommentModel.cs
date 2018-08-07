using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Components.Discussions.Models
{
    public class AddCommentModel
    {
        [Required]
        public string CommentText { get; set; }
        public string AddCommentActionPath { get; set; }
    }
}