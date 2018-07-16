using System;
using System.Collections.Generic;

namespace Ubora.Web._Features.Projects._Shared
{
    public class ProjectMenuViewModel
    {
        public ProjectMenuViewModel(ProjectMenuOption activeMenuOption)
        {
            ActiveMenuOption = activeMenuOption;
        }

        public ProjectMenuOption ActiveMenuOption { get; }

        private static Dictionary<ProjectMenuOption, string> MenuOptionNamesInner = new Dictionary<ProjectMenuOption, string>
        {
            {ProjectMenuOption.Overview, "Overview"},
            {ProjectMenuOption.Workpackages, "Work packages"},
            {ProjectMenuOption.Assignments, "Assignments"},
            {ProjectMenuOption.Repository, "Repository"},
            {ProjectMenuOption.Members, "Members"},
            {ProjectMenuOption.History, "History"}
        };

        public Dictionary<ProjectMenuOption, string> MenuOptionNames => MenuOptionNamesInner;
    }
}