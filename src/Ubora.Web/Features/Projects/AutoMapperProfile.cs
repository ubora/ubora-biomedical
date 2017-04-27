using AutoMapper;
using Ubora.Domain.Projects;

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
