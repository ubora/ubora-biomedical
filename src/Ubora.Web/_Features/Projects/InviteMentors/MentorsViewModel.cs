using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Web.Infrastructure.Extensions;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Web._Features.UboraMentors.Queries;
using Ubora.Web._Features.Users.UserList;

namespace Ubora.Web._Features.Projects.InviteMentors
{
    public class MentorsViewModel
    {
        public IEnumerable<UserListItemViewModel> UboraMentors { get; set; }
        public IEnumerable<UserListItemViewModel> ProjectMentors { get; set; }

        public class Factory
        {
            private readonly IQueryProcessor _queryProcessor;
            private readonly IMapper _autoMapper;
            private readonly ImageStorageProvider _imageStorageProvider;

            public Factory(IQueryProcessor queryProcessor, IMapper autoMapper, ImageStorageProvider imageStorageProvider)
            {
                _queryProcessor = queryProcessor;
                _autoMapper = autoMapper;
                _imageStorageProvider = imageStorageProvider;
            }

            protected Factory()
            {
            }

            public virtual MentorsViewModel Create(Guid projectId)
            {
                var projectMentors = _queryProcessor.ExecuteQuery(new FindProjectMentorProfilesQuery
                {
                    ProjectId = projectId
                });
                var projectMentorIds = projectMentors.Select(x => x.UserId);

                var uboraMentors = _queryProcessor.ExecuteQuery(new FindUboraMentorProfilesQuery())
                    .ToList();

                uboraMentors.RemoveAll(x => projectMentorIds.Contains(x.UserId));

                var model = new MentorsViewModel
                {
                    UboraMentors = uboraMentors.Select(x => new UserListItemViewModel
                    {
                        UserId = x.UserId,
                        Email = x.Email,
                        FullName = x.FullName,
                        ProfilePictureLink = _imageStorageProvider.GetDefaultOrBlobUrl(x)
                    }),
                    ProjectMentors = projectMentors.Select(x => new UserListItemViewModel
                    {
                        UserId = x.UserId,
                        Email = x.Email,
                        FullName = x.FullName,
                        ProfilePictureLink = _imageStorageProvider.GetDefaultOrBlobUrl(x)
                    })
                };

                return model;
            }
        }
    }
}