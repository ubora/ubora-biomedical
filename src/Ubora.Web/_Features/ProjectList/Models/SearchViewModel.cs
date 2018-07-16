using System.Linq;

namespace Ubora.Web._Features.ProjectList.Models
{
    public class SearchViewModel : SearchModel
    {
        public bool IsAnyFilterSet => ByArea.Any() || ByClinicalNeedTags.Any() || ByPotentialTechnologyTags.Any() || ByStatus != ByStatusFilteringMethod.All;
        public ProjectListViewModel ProjectListViewModel { get; set; }
    }
}
