using AutoMapper;

namespace Ubora.Domain.Projects
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateProjectCommand, ProjectCreatedEvent>()
                .DisableCtorValidation();
        }
    }
}
