using AutoMapper;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Tasks;
using Ubora.Web.Features.ProjectManagement;
using Ubora.Web.Features.ProjectManagement.Tasks;
using Ubora.Web.Features.Projects;

namespace Ubora.Web.Features.Shared
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateViewModel, CreateProjectCommand>()
                .ForMember(m => m.UserInfo, opt => opt.Ignore())
                .ForMember(m => m.Id, opt => opt.Ignore());

            CreateMap<AddTaskViewModel, AddTaskCommand>()
                .ForMember(m => m.Id, opt => opt.Ignore())
                .ForMember(m => m.InitiatedBy, opt => opt.Ignore());

            CreateMap<Project, ListItemViewModel>();

            CreateMap<ProjectTask, TaskListItemViewModel>();
            CreateMap<ProjectTask, EditTaskViewModel>();
            CreateMap<EditTaskViewModel, EditTaskCommand>()
                .ForMember(m => m.InitiatedBy, opt => opt.Ignore());

            CreateMap<Project, StepOneViewModel>();

            CreateMap<Project, StepTwoViewModel>();

            CreateMap<Project, UpdateProjectCommand>()
                .ForMember(m => m.UserInfo, opt => opt.Ignore());
        }
    }
}
