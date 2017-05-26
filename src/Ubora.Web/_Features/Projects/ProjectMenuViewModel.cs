using System;

namespace Ubora.Web._Features.Projects
{
    public class ProjectMenuViewModel
    {
        private const string ActiveMenuOptionCssClass = "project_navigation_link--active";

        public ProjectMenuViewModel(ProjectMenuOption option)
        {
            //var option = Enum.Parse(typeof(ProjectMenuOption), menuOptionEnumString);

            switch (option)
            {
                case ProjectMenuOption.Overview:
                    OverviewActivityCssClass = ActiveMenuOptionCssClass;
                    break;
                case ProjectMenuOption.Workpackages:
                    WorkpackagesActivityCssClass = ActiveMenuOptionCssClass;
                    break;
                case ProjectMenuOption.Tasks:
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

    public enum ProjectMenuOption
    {
        Overview,
        Workpackages,
        Tasks,
        News,
        Documentation,
        Repository,
        History,
        Members
    }
}
