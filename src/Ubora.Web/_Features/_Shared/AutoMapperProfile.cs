using AutoMapper;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Tasks;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Web._Features.ProjectList;
using Ubora.Domain.Users;
using Ubora.Web._Features.Projects.Dashboard;
using Ubora.Web._Features.Projects.Tasks;
using Ubora.Web._Features.Projects.Workpackages.Reviews;
using Ubora.Web._Features.Projects.Workpackages.Steps;
using Ubora.Web._Features.Users.Profile;
using Ubora.Web._Features.Users.UserList;
using Ubora.Web._Features.Projects.Repository;
using Ubora.Domain.Projects.Repository;

namespace Ubora.Web._Features._Shared
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProjectTask, TaskListItemViewModel>();
            CreateMap<ProjectTask, EditTaskViewModel>();

            CreateMap<ProjectFile, ProjectFileViewModel>()
                .ForMember(dest => dest.FileLocation, o => o.Ignore());

            CreateMap<Project, ProjectListViewModel.ProjectListItem>();

            CreateMap<Project, UpdateProjectCommand>()
                .ForMember(dest => dest.ProjectId, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Actor, o => o.Ignore());

            CreateMap<WorkpackageStep, StepViewModel>()
                .ForMember(dest => dest.StepId, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.ReadStepUrl, o => o.Ignore())
                .ForMember(dest => dest.EditStepUrl, o => o.Ignore());

            CreateMap<UserProfile, UserListItemViewModel>(MemberList.None);
            CreateMap<UserProfile, ProfileViewModel>(MemberList.None);
            CreateMap<UserProfile, UserProfileViewModel>().ForMember(dest => dest.CountryCode, o => o.MapFrom(src => src.Country.Code));

            CreateMap<Project, DesignPlanningViewModel>();
            CreateMap<Project, ProjectDashboardViewModel>()
                .ForMember(dest => dest.IsProjectMember, o => o.Ignore());

            CreateMap<WorkpackageReview, WorkpackageReviewViewModel>();
        }
    }
}
