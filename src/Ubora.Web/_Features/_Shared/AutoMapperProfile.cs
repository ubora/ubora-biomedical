using AutoMapper;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.DeviceClassification;
using Ubora.Domain.Projects.Tasks;
using Ubora.Web._Features.ProjectList;
using Ubora.Web._Features.Projects.Tasks;
using Ubora.Web._Features.Projects.Workpackages;

namespace Ubora.Web._Features._Shared
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProjectTask, TaskListItemViewModel>();
            CreateMap<ProjectTask, EditTaskViewModel>();

            CreateMap<Project, ProjectListViewModel.ProjectListItem>();
            CreateMap<Project, StepOneViewModel>();
            CreateMap<Project, StepTwoViewModel>();

            CreateMap<Project, UpdateProjectCommand>()
                .ForMember(dest => dest.ProjectId, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Actor, o => o.Ignore());

            CreateMap<Project, SaveDeviceClassificationToProjectCommand>()
                .ForMember(m => m.Actor, opt => opt.Ignore())
                .ForMember(m => m.Id, opt => opt.Ignore());
        }
    }
}
