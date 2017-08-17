using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Ubora.Domain.Infrastructure.Queries;
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

            public Factory(IQueryProcessor queryProcessor, IMapper autoMapper)
            {
                _queryProcessor = queryProcessor;
                _autoMapper = autoMapper;
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
                    UboraMentors = uboraMentors.Select(_autoMapper.Map<UserListItemViewModel>),
                    ProjectMentors = projectMentors.Select(_autoMapper.Map<UserListItemViewModel>)
                };

                return model;
            }
        }
    }
}