using System;
using System.Collections.Generic;

namespace Ubora.Web.Areas.Projects.Models.ProjectList
{
    public class ProjectListViewModel
    {
        public IEnumerable<ProjectListItemViewModel> Projects { get; set; }
    }

    public class ProjectListItemViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}