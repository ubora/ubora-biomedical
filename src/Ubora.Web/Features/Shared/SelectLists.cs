using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ubora.Web.Features.Shared
{
    public static class SelectLists
    {
        public static List<SelectListItem> UserRoles => new List<SelectListItem>(new[]
        {
            new SelectListItem { Value = "I am a...", Text = "I am a...", Disabled = true, Selected = true },
            new SelectListItem { Value = "Student", Text = "Student"},
            new SelectListItem { Value = "Professor", Text = "Professor"},
            new SelectListItem { Value = "Mentor", Text = "Mentor"},
            new SelectListItem { Value = "Specialist/Expert", Text = "Specialist/Expert"},
        });
    }
}
