using System.Collections.Generic;

namespace Ubora.Web.Features.Projects.List
{
    public class ListViewModel
    {
        public IEnumerable<ListItemViewModel> Projects { get; set; }
    }
}