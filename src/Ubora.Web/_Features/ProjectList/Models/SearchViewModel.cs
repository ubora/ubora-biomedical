using System.Collections.Generic;
using System.Linq;
using Ubora.Web._Features._Shared;

namespace Ubora.Web._Features.ProjectList.Models
{
    public class SearchViewModel
    {
        public string Title { get; set; }
        public TabType Tab { get; set; }
        public string ByPotentialTechnologyTags { get; set; }
        public string ByClinicalNeedTags { get; set; }
        public string ByArea { get; set; }
        public ByStatusFilteringMethod ByStatus { get; set; }
        public SortBy SortBy { get; set; }
        public ProjectListViewModel ProjectListViewModel { get; set; }
        public List<string> PotentialTechnologyTags { get; set; } = SelectLists.PotentialTechnologyTags.Select(a => a.Text).ToList();
        public List<string> ClinicalNeedTags { get; set; } = SelectLists.ClinicalNeedTags.Select(a => a.Text).ToList();
        public List<string> AreaOfUsageTags { get; set; } = SelectLists.Areas.Select(a => a.Text).ToList();
    }
}
