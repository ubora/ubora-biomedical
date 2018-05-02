using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Features.ProjectList
{
    public class SearchViewModel
    {
        [StringLength(50)]
        public string Title { get; set; }
        public int Page { get; set; } = 1;
    }
}
