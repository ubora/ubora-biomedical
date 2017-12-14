using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.Members;
using Ubora.Domain.Projects.Members.Queries;
using Ubora.Domain.Projects._Queries;
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
        public IEnumerable<UserProject> UserProjects { get; set; }

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

                var userProjects = _queryProcessor.ExecuteQuery(new FindUserProjectsQuery {UserId = userProfile.UserId})
                    .OrderBy(x => x.Title);

                viewModel.UserProjects = userProjects.Select(project => new UserProject
                {
                    ProjectId = project.Id,
                    Title = project.Title,
                    IsLeader = project.HasMember<ProjectMentor>(userProfile.UserId),
                    IsMentor = project.HasMember<ProjectLeader>(userProfile.UserId)
                });

                if (userProfile.Role == "Mentor" && !viewModel.IsVerifiedMentor)
                {
                    viewModel.IsUnverifedMentor = true;
                }

                return viewModel;
            }
        }
    }

    public class UserProject
    {
        public Guid ProjectId { get; set; }
        public string Title { get; set; }
        public bool IsLeader { get; set; }
        public bool IsMentor { get; set; }
    }
}
