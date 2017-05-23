using AutoMapper;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Tasks;
using Ubora.Web._Features.ProjectList;
using Ubora.Web._Features.Projects.Tasks;

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
        }
    }
}
