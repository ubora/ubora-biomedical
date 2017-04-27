using System;
using System.Collections.Generic;

namespace Ubora.Web.Features.Projects.Dashboard
{
    public class DashboardViewModel : WorkPackageOneViewModel
    {
        public IEnumerable<string> EventStream { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Members { get; set; }
        public string ProjectData { get; set; }
    }

    public class WorkPackageOneViewModel
    {
        public string DescriptionOfNeed { get; set; }
        public string DescriptionOfExistingSolutionsAndAnalysis { get; set; }
        public string ProductSpecification { get; set; }
        public string ProductPerformance { get; set; }
        public string ProductUsability { get; set; }
        public string ProductSafety { get; set; }
        public string PatientsTargetGroup { get; set; }
        public string EndusersTargetGroup { get; set; }
        public string AdditionalInformation { get; set; }
    }
}