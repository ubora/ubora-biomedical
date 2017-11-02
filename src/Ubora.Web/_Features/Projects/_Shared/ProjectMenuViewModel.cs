using System;

namespace Ubora.Web._Features.Projects._Shared
{
    public class ProjectMenuViewModel
    {
        private const string ActiveMenuOptionCssClass = "active";

        public ProjectMenuViewModel(ProjectMenuOption option)
        {
            switch (option)
            {
                case ProjectMenuOption.Overview:
                    OverviewActivityCssClass = ActiveMenuOptionCssClass;
                    break;
                case ProjectMenuOption.Workpackages:
                    WorkpackagesActivityCssClass = ActiveMenuOptionCssClass;
                    break;
                case ProjectMenuOption.Assignments:
                    TasksActivityCssClass = ActiveMenuOptionCssClass;
                    break;
                case ProjectMenuOption.News:
                    NewsActivityCssClass = ActiveMenuOptionCssClass;
                    break;
                case ProjectMenuOption.Documentation:
                    DocumentationActivityCssClass = ActiveMenuOptionCssClass;
                    break;
                case ProjectMenuOption.Repository:
                    RepositoryActivityCssClass = ActiveMenuOptionCssClass;
                    break;
                case ProjectMenuOption.History:
                    HistoryActivityCssClass = ActiveMenuOptionCssClass;
                    break;
                case ProjectMenuOption.Members:
                    MembersActivityCssClass = ActiveMenuOptionCssClass;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(option), option, null);
            }
        }

        public string OverviewActivityCssClass { get; }
        public string WorkpackagesActivityCssClass { get; }
        public string TasksActivityCssClass { get; }
        public string NewsActivityCssClass { get; }
        public string DocumentationActivityCssClass { get; }
        public string RepositoryActivityCssClass { get; }
        public string HistoryActivityCssClass { get; }
        public string MembersActivityCssClass { get; }
    }
}
