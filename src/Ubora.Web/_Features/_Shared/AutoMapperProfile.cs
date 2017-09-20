using AutoMapper;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Tasks;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Web._Features.ProjectList;
using Ubora.Domain.Users;
using Ubora.Web._Features.Projects.Dashboard;
using Ubora.Web._Features.Projects.Workpackages.Reviews;
using Ubora.Web._Features.Projects.Workpackages.Steps;
using Ubora.Web._Features.Users.Profile;
using Ubora.Web._Features.Users.UserList;
using Ubora.Web._Features.Projects.Repository;
using Ubora.Domain.Projects.Repository;
using Ubora.Web._Features.Projects.Assignments;

namespace Ubora.Web._Features._Shared
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProjectTask, AssignmentListItemViewModel>();
            CreateMap<ProjectTask, EditAssignmentViewModel>();

            CreateMap<ProjectFile, ProjectFileViewModel>();

            CreateMap<ProjectFile, UpdateFileViewModel>()
                .ForMember(dest => dest.ProjectFile, o => o.Ignore())
                .ForMember(dest => dest.FileId, o => o.MapFrom(src => src.Id));

            CreateMap<Project, ProjectListViewModel.ProjectListItem>()
                .ForMember(dest => dest.ImagePath, o => o.Ignore());

            CreateMap<Project, UpdateProjectCommand>()
                .ForMember(dest => dest.ProjectId, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Actor, o => o.Ignore());

            CreateMap<WorkpackageStep, EditStepViewModel>()
                .ForMember(dest => dest.StepId, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.ReadStepUrl, o => o.Ignore())
                .ForMember(dest => dest.EditStepUrl, o => o.Ignore());

            CreateMap<WorkpackageStep, ReadStepViewModel>()
                .IncludeBase<WorkpackageStep, EditStepViewModel>()
                .ForMember(dest => dest.EditButton, o => o.Ignore());

            CreateMap<UserProfile, UserListItemViewModel>()
                .ForMember(dest => dest.ProfilePictureLink, o => o.Ignore());

            CreateMap<UserProfile, ProfileViewModel>()
                .ForMember(dest => dest.ProfilePictureLink, o => o.Ignore());

            CreateMap<UserProfile, UserProfileViewModel>().ForMember(dest => dest.CountryCode, o => o.MapFrom(src => src.Country.Code));

            CreateMap<Project, ProjectOverviewViewModel>();
            CreateMap<Project, ProjectDashboardViewModel>()
                .ForMember(dest => dest.IsProjectMember, o => o.Ignore())
                .ForMember(dest => dest.ImagePath, o => o.Ignore())
                .ForMember(dest => dest.HasImage, o => o.Ignore());

            CreateMap<WorkpackageReview, WorkpackageReviewViewModel>();

            CreateMap<Project, DeviceClassificationViewModel>();
        }
    }
}
