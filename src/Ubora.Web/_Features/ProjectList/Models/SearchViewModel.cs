using System.Collections.Generic;
using System.Linq;
using Ubora.Web._Features._Shared;

namespace Ubora.Web._Features.ProjectList.Models
{
    public class SearchViewModel
    {
        public string Title { get; set; }
        public TabType Tab { get; set; }
        public int[] ByPotentialTechnologyTags { get; set; }
        public int[] ByClinicalNeedTags { get; set; }
        public int[] ByArea { get; set; }
        public ByStatusFilteringMethod ByStatus { get; set; }
        public SortBy SortBy { get; set; }
        public ProjectListViewModel ProjectListViewModel { get; set; }
        
        
    }
}
