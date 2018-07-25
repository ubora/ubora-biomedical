using System.Linq;

namespace Ubora.Web._Features.ProjectList.Models
{
    public class SearchViewModel : SearchModel
    {
        public bool IsAnyFilterSet => 
            ByArea.Any() 
            || ByClinicalNeedTags.Any() 
            || ByPotentialTechnologyTags.Any() 
            || ByStatus != ByStatusFilteringMethod.All 
            || !string.IsNullOrWhiteSpace(Title);
        public ProjectListViewModel ProjectListViewModel { get; set; }
    }
}
