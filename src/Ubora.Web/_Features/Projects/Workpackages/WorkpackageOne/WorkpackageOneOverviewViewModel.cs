using System;
using System.Collections.Generic;

namespace Ubora.Web._Features.Projects.Workpackages.WorkpackageOne
{
    public class WorkpackageOneOverviewViewModel
    {
        public string Title { get; set; }
        public IEnumerable<Step> Steps { get; set; }

        public class Step
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public bool IsSelected { get; set; }
        }
    }
}