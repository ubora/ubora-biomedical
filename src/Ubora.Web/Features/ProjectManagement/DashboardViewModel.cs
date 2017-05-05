using System;
using System.Collections.Generic;

namespace Ubora.Web.Features.ProjectManagement
{
    public class DashboardViewModel
    {
        public IEnumerable<string> EventStream { get; set; }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> Members { get; set; }
    }
}