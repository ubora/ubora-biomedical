using System;
using System.Collections.Generic;

namespace Ubora.Web.Areas.Projects.Models.ProjectDashboard
{
    public class ProjectDashboardViewModel
    {
        public IEnumerable<string> EventStream { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}