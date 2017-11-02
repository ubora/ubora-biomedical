using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members.Queries;

namespace Ubora.Web._Features.Projects.Assignments
{
    public class AddAssignmentViewModel
    {
        public Guid ProjectId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public Guid[] AssigneeIds { get; set; }
        public IEnumerable<TaskAssigneeViewModel> ProjectMembers { get; set; }

        public class Factory
        {
            private readonly IQueryProcessor _queryProcessor;

            public Factory(IQueryProcessor queryProcessor)
            {
                _queryProcessor = queryProcessor;
            }

            public AddAssignmentViewModel Create(Guid projectId)
            {
                var project = _queryProcessor.FindById<Project>(projectId);
                var projectMembersIds = project.Members.Select(x => x.UserId);
                var projectMembers = _queryProcessor.ExecuteQuery(new FindUserProfilesQuery
                {
                    UserIds = projectMembersIds
                });
                var taskAssigneeViewModel = projectMembers.Select(x => new TaskAssigneeViewModel
                {
                    AssigneeId = x.UserId,
                    FullName = x.FullName
                });

                var model = new AddAssignmentViewModel
                {
                    ProjectId = projectId,
                    ProjectMembers = taskAssigneeViewModel
                };

                return model;
            }
        }
    }
}