using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.Workpackages;
using System.Collections.Generic;

namespace Ubora.Web._Features.Projects.Workpackages
{
    public class WorkpackageMenuViewModel
    {
        public string WorkPackageId { get; set; }
        public string Title { get; set; }
        public IEnumerable<MenuLink> MenuLinks { get; set; }
        public bool HasBeenAccepted { get; set; }
        public bool IsMuted { get; set; }

        public string IconOpenSrc { get; set; }
        public string IconMutedSrc { get; set; }

        public string WorkPackageImageSrc
        {
            get
            {
                if (IsMuted)
                {
                    return IconMutedSrc;
                }

                if (HasBeenAccepted)
                {
                    return "/images/icons/check.svg";
                }

                return IconOpenSrc;
            }
        }

        public bool IsAnyMenuLinkSelected
        {
            get
            {
                return MenuLinks.Any(x => x.IsSelected);
            }
        }
    }

    public class MenuLink
    {
        public bool IsSelected { get; set; }
        public string Href { get; set; }
        public string Name { get; set; }
    }

    public class WorkpackageListOverviewViewComponent : ProjectViewComponent
    {
        public WorkpackageListOverviewViewComponent(IQueryProcessor processor)
            : base(processor)
        {
        }

#pragma warning disable 1998
        public async Task<IViewComponentResult> InvokeAsync()
#pragma warning restore 1998
        {
            var workPackageMenuViewModels = new List<WorkpackageMenuViewModel>();
            workPackageMenuViewModels.Add(CreateWorkpackageOneMenuViewModel());
            workPackageMenuViewModels.Add(CreateWorkpackageTwoMenuViewModel());
            workPackageMenuViewModels.Add(CreateWorkpackageThreeMenuViewModel());
            workPackageMenuViewModels.Add(CreateWorkpackageFourMenuViewModel());
            workPackageMenuViewModels.Add(CreateWorkpackageFiveMenuViewModel());
            workPackageMenuViewModels.Add(CreateWorkpackageSixMenuViewModel());

            return View("~/_Features/Projects/Workpackages/_WorkpackageListOverviewPartial.cshtml", workPackageMenuViewModels);
        }

        private WorkpackageMenuViewModel CreateWorkpackageOneMenuViewModel()
        {
            var workpackage = QueryProcessor.FindById<WorkpackageOne>(ProjectId);
            if (workpackage == null)
            {
                return new WorkpackageMenuViewModel
                {
                    Title = "Medical need and product specification",
                    WorkPackageId = "workPackageOne",
                    MenuLinks = new List<MenuLink>(),
                    IsMuted = true,
                    IconMutedSrc = "/images/icons/one_muted.svg"
                };
            }

            var menuLinks = workpackage.Steps.Select(step => new MenuLink
            {
                Name = step.Title,
                IsSelected = GetSelectedStepId() == step.Id,
                Href = Url.Action("Read", "WorkpackageOne", new { projectId = ProjectId, stepId = step.Id })
            }).ToList();

            menuLinks.Add(new MenuLink
            {
                Name = "Device classification",
                Href = Url.Action("DeviceClassification", "WorkpackageOne"),
                IsSelected = IsWorkpackageCustomMenuOptionSelected(WorkpackageMenuOption.DeviceClassification)
            });

            menuLinks.Add(new MenuLink
            {
                Name = "Regulation checklist",
                Href = Url.Action("Index", "ApplicableRegulations"),
                IsSelected = IsWorkpackageCustomMenuOptionSelected(WorkpackageMenuOption.RegulationCheckList)
            });

            menuLinks.Add(new MenuLink
            {
                Name = "Mentor review",
                Href = Url.Action("Review", "WorkpackageOneReview"),
                IsSelected = IsWorkpackageCustomMenuOptionSelected(WorkpackageMenuOption.Wp1MentorReview)
            });

            var workPackageMenu = new WorkpackageMenuViewModel
            {
                Title = workpackage.Title,
                WorkPackageId = "workpackageOne",
                MenuLinks = menuLinks,
                IconOpenSrc = "/images/icons/one_open.svg",
                IconMutedSrc = "/images/icons/one_muted.svg",
                HasBeenAccepted = workpackage.HasBeenAccepted
            };

            return workPackageMenu;
        }

        private WorkpackageMenuViewModel CreateWorkpackageTwoMenuViewModel()
        {
            var workpackage = QueryProcessor.FindById<WorkpackageTwo>(ProjectId);
            if (workpackage == null)
            {
                return new WorkpackageMenuViewModel
                {
                    Title = "Conceptual design",
                    WorkPackageId = "workPackageTwo",
                    MenuLinks = new List<MenuLink>(),
                    IsMuted = true,
                    IconMutedSrc = "/images/icons/two_muted.svg"
                };
            }

            var menuLinks = workpackage.Steps.Select(step => new MenuLink
            {
                Name = step.Title,
                IsSelected = GetSelectedStepId() == step.Id,
                Href = Url.Action("Read", "WorkpackageTwo", new { projectId = ProjectId, stepId = step.Id })
            }).ToList();

            menuLinks.Add(new MenuLink
            {
                Name = "Mentor review",
                Href = Url.Action("Review", "WorkpackageTwoReview"),
                IsSelected = IsWorkpackageCustomMenuOptionSelected(WorkpackageMenuOption.Wp2MentorReview)
            });

            var workPackageMenu = new WorkpackageMenuViewModel
            {
                Title = workpackage.Title,
                WorkPackageId = "workpackageTwo",
                MenuLinks = menuLinks,
                IconOpenSrc = "/images/icons/two_open.svg",
                IconMutedSrc = "/images/icons/two_muted.svg",
                HasBeenAccepted = workpackage.HasBeenAccepted
            };

            return workPackageMenu;
        }

        private WorkpackageMenuViewModel CreateWorkpackageThreeMenuViewModel()
        {
            var workpackage = QueryProcessor.FindById<WorkpackageThree>(ProjectId);
            if (workpackage == null)
            {
                return new WorkpackageMenuViewModel
                {
                    Title = "Design and prototyping",
                    WorkPackageId = "workpackageThree",
                    MenuLinks = new List<MenuLink>(),
                    IsMuted = true,
                    IconMutedSrc = "/images/icons/three_muted.svg"
                };
            }

            var menuLinks = workpackage.Steps.Select(step => new MenuLink
            {
                Name = step.Title,
                IsSelected = GetSelectedStepId() == step.Id,
                Href = Url.Action("Read", "WorkpackageThree", new { projectId = ProjectId, stepId = step.Id })
            }).ToList();

            var workPackageMenu = new WorkpackageMenuViewModel
            {
                Title = workpackage.Title,
                WorkPackageId = "workpackageThree",
                MenuLinks = menuLinks,
                IconOpenSrc = "/images/icons/three_open.svg",
                IconMutedSrc = "/images/icons/three_muted.svg",
                HasBeenAccepted = workpackage.HasBeenAccepted
            };

            return workPackageMenu;
        }

        private WorkpackageMenuViewModel CreateWorkpackageFourMenuViewModel()
        {
            return new WorkpackageMenuViewModel
            {
                Title = "Implementation",
                WorkPackageId = "workpackageFour",
                MenuLinks = new List<MenuLink>(),
                IsMuted = true,
                IconMutedSrc = "/images/icons/four_muted.svg"
            };
        }

        private WorkpackageMenuViewModel CreateWorkpackageFiveMenuViewModel()
        {
            return new WorkpackageMenuViewModel
            {
                Title = "Operation",
                WorkPackageId = "workpackageFive",
                MenuLinks = new List<MenuLink>(),
                IsMuted = true,
                IconMutedSrc = "/images/icons/five_muted.svg"
            };
        }

        private WorkpackageMenuViewModel CreateWorkpackageSixMenuViewModel()
        {
            return new WorkpackageMenuViewModel
            {
                Title = "Project closure",
                WorkPackageId = "workpackageSix",
                MenuLinks = new List<MenuLink>(),
                IsMuted = true,
                IconMutedSrc = "/images/icons/six_muted.svg"
            };
        }

        private string GetSelectedStepId()
        {
            return RouteData.Values["stepId"] as string;
        }

        private bool IsWorkpackageCustomMenuOptionSelected(WorkpackageMenuOption option)
        {
            var menuOption = ViewData["WorkpackageMenuOption"];
            if (menuOption == null)
            {
                return false;
            }

            return (WorkpackageMenuOption)menuOption == option;
        }
    }
}