using AutoMapper;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Tasks;
using Ubora.Web.Features.ProjectManagement.Tasks;

namespace Ubora.Web.Features.Projects
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreatePostModel, CreateProjectCommand>()
                .ForMember(m => m.UserInfo, opt => opt.Ignore())
                .ForMember(m => m.ProjectId, opt => opt.Ignore());

            CreateMap<AddTaskViewModel, AddTaskCommand>()
                .ForMember(m => m.TaskId, opt => opt.Ignore())
                .ForMember(m => m.InitiatedBy, opt => opt.Ignore());

            CreateMap<ProjectTask, TaskListItemViewModel>();
            CreateMap<ProjectTask, EditTaskViewModel>();
        }
    }
}
