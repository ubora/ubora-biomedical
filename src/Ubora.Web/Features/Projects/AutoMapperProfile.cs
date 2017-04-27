using AutoMapper;
using Ubora.Domain.Projects;
using Ubora.Web.Features.Projects.Create;

namespace Ubora.Web.Features.Projects
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreatePostModel, CreateProjectCommand>();
        }
    }
}
