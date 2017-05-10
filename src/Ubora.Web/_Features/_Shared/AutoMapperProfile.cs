using AutoMapper;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Tasks;
using Ubora.Web._Features.Projects.Creation;
using Ubora.Web._Features.Projects.List;
using Ubora.Web._Features.Projects.Tasks;
using Ubora.Web._Features.Projects.Workpackages;

namespace Ubora.Web._Features._Shared
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateProjectViewModel, CreateProjectCommand>()
                .ForMember(m => m.UserInfo, opt => opt.Ignore())
                .ForMember(m => m.Id, opt => opt.Ignore());

            CreateMap<AddTaskViewModel, AddTaskCommand>()
                .ForMember(m => m.Id, opt => opt.Ignore())
                .ForMember(m => m.UserInfo, opt => opt.Ignore());

            CreateMap<Project, ProjectListViewModel.ProjectListItem>();

            CreateMap<ProjectTask, TaskListItemViewModel>();
            CreateMap<ProjectTask, EditTaskViewModel>();
            CreateMap<EditTaskViewModel, EditTaskCommand>()
                .ForMember(m => m.UserInfo, opt => opt.Ignore());

            CreateMap<Project, StepOneViewModel>();

            CreateMap<Project, StepTwoViewModel>();

            CreateMap<Project, UpdateProjectCommand>()
                .ForMember(m => m.UserInfo, opt => opt.Ignore());
        }
    }
}
