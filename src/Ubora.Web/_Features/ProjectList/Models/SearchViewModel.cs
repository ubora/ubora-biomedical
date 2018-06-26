using System.Collections.Generic;

namespace Ubora.Web._Features.ProjectList.Models
{
    public class SearchViewModel
    {
        public string Title { get; set; }
        public TabType Tab { get; set; }
        public string ByArea { get; set; }
        public ByStatusFilteringMethod ByStatus { get; set; }
        public SortBy SortBy { get; set; }
        public ProjectListViewModel ProjectListViewModel { get; set; }
        public List<string> AreaOfUsageTags { get; set; } =
            new List<string> { "Cardiovascular surgery",
                "Colorectal surgery",
                "General surgery",
                "Neurosurgery",
                "Oncologic surgery",
                "Ophthalmic surgery",
                "Oral and maxillofacial surgery",
                "Orthopedic surgery"
            };
    }
}
