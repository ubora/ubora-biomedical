using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ubora.Web._Features._Shared
{
    public static class SelectLists
    {
        public static List<SelectListItem> UserRoles => new List<SelectListItem>(new[]
        {
            new SelectListItem { Value = "I am a...", Text = "I am a...", Disabled = true, Selected = true },
            new SelectListItem { Value = "Student", Text = "Student"},
            new SelectListItem { Value = "Professor", Text = "Professor"},
            new SelectListItem { Value = "Mentor", Text = "Mentor"},
            new SelectListItem { Value = "Specialist/Expert", Text = "Specialist/Expert"}
        });

        public static List<SelectListItem> UserMedicalDevices => new List<SelectListItem>(new[]
        {
            new SelectListItem { Value = "Select from the list...", Text = "Select from the list...", Disabled = true, Selected = true },
            new SelectListItem { Value = "Student", Text = "Student"},
            new SelectListItem { Value = "Researcher", Text = "Researcher"},
            new SelectListItem { Value = "Professional Designer", Text = "Professional Designer"},
            new SelectListItem { Value = "Healthcare provider", Text = "Healthcare provider"},
            new SelectListItem { Value = "Consultant", Text = "Consultant"},
            new SelectListItem { Value = "Other", Text = "Other"}
        });
    }
}
