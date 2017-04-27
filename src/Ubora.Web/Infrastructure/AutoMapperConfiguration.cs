using AutoMapper;
using Ubora.Domain.Projects;
using Ubora.Web.Areas.Projects.Views.Create;

namespace Ubora.Web.Infrastructure
{
    public class WebAutoMapperProfile : Profile
    {
        public WebAutoMapperProfile()
        {
            

            CreateMap<CreatePostModel, CreateProjectCommand>();
        }
    }
}
