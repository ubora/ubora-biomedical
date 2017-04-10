using System.Collections.Generic;

namespace Ubora.Web.Areas.Projects.Views.List
{
    public class ListViewModel
    {
        public IEnumerable<ListItemViewModel> Projects { get; set; }
    }
}