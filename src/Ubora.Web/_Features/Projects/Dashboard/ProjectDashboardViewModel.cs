using Microsoft.AspNetCore.Authorization;
using Ubora.Web.Data;

namespace Ubora.Web._Features.Projects.Dashboard
{
    public class ProjectDashboardViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string DeviceClassification { get; set; }
        public string ClinicalNeedTags { get; set; }
        public string AreaOfUsageTags { get; set; }
        public string PotentialTechnologyTags { get; set; }
        public string Gmdn { get; set; }
        public bool IsProjectMember { get; set; }
        public bool IsInDraft { get; set; }
        public string ImagePath { get; set; }
        public bool HasImage { get; set; }
       
    }
}