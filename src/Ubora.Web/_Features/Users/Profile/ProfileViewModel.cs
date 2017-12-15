using AutoMapper;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Users;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Web._Features.UboraMentors.Queries;
using Ubora.Web.Infrastructure.Extensions;

namespace Ubora.Web._Features.Users.Profile
{
    public class ProfileViewModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string University { get; set; }
        public string Degree { get; set; }
        public string Field { get; set; }
        public string Biography { get; set; }
        public string Skills { get; set; }
        public string Role { get; set; }
        public bool IsVerifiedMentor { get; set; }
        public bool IsUnverifedMentor { get; set; }
        public string ProfilePictureLink { get; set; }
        public string FullName { get; set; }
        public bool IsDeleted { get; set; }
        public string CountryEnglishName { get; set; }
        public string Institution { get; set; }
        public string MedicalDevice { get; set; }

        public class Factory
        {
            private readonly IMapper _autoMapper;
            private readonly ImageStorageProvider _imageStorageProvider;
            private readonly IQueryProcessor _queryProcessor;

            public Factory(IMapper autoMapper, ImageStorageProvider imageStorageProvider, IQueryProcessor queryProcessor)
            {
                _autoMapper = autoMapper;
                _imageStorageProvider = imageStorageProvider;
                _queryProcessor = queryProcessor;
            }

            protected Factory()
            {
            }

            public virtual ProfileViewModel Create(UserProfile userProfile)
            {
                var viewModel = _autoMapper.Map<ProfileViewModel>(userProfile);

                viewModel.ProfilePictureLink = _imageStorageProvider.GetDefaultOrBlobUrl(userProfile);

                viewModel.IsVerifiedMentor = _queryProcessor.ExecuteQuery(new IsVerifiedUboraMentorQuery(userProfile.UserId));

                if (userProfile.Role == "Mentor" && !viewModel.IsVerifiedMentor)
                {
                    viewModel.IsUnverifedMentor = true;
                }

                return viewModel;
            }
        }
    }
}
