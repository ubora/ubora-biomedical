using AutoMapper;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Tasks;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Web._Features.ProjectList;
using Ubora.Domain.Users;
using Ubora.Web._Features.Projects.Dashboard;
using Ubora.Web._Features.Projects.Tasks;
using Ubora.Web._Features.Users.Profile;
using Ubora.Web._Features.Users.UserList;
using Ubora.Web._Features.Projects.Workpackages.WorkpackageOne;
using Ubora.Web._Features.Projects.Workpackages.WorkpackageOneReview;

namespace Ubora.Web._Features._Shared
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProjectTask, TaskListItemViewModel>();
            CreateMap<ProjectTask, EditTaskViewModel>();

            CreateMap<Project, ProjectListViewModel.ProjectListItem>();

            CreateMap<Project, UpdateProjectCommand>()
                .ForMember(dest => dest.ProjectId, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Actor, o => o.Ignore());

            CreateMap<WorkpackageStep, StepViewModel>()
                .ForMember(dest => dest.StepId, o => o.MapFrom(src => src.Id));

            CreateMap<UserProfile, UserListItemViewModel>(MemberList.None);
            CreateMap<UserProfile, ProfileViewModel>(MemberList.None);
            CreateMap<UserProfile, UserProfileViewModel>();

            CreateMap<Project, DesignPlanningViewModel>();
            CreateMap<Project, ProjectDashboardViewModel>()
                .ForMember(dest => dest.IsProjectMember, o => o.Ignore());

            CreateMap<WorkpackageReview, WorkpackageReviewViewModel>();
        }
    }
}
