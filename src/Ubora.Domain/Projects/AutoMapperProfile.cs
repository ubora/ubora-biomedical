using AutoMapper;
using Ubora.Domain.Projects.Tasks;

namespace Ubora.Domain.Projects
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateProjectCommand, ProjectCreatedEvent>()
                .DisableCtorValidation();

            CreateMap<AddTaskCommand, TaskAddedEvent>()
                .DisableCtorValidation();
        }
    }
}
