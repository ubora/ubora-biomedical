using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Assignments;
using Ubora.Domain.Projects.Members.Queries;

namespace Ubora.Web._Features.Projects.Assignments
{
    public class EditAssignmentViewModel
    {
        public Guid ProjectId { get; set; }
        public Guid Id { get; set; }
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
            private readonly IMapper _autoMapper;

            public Factory(IQueryProcessor queryProcessor, IMapper autoMapper)
            {
                _queryProcessor = queryProcessor;
                _autoMapper = autoMapper;
            }

            public EditAssignmentViewModel Create(Guid taskId)
            {
                var task = _queryProcessor.FindById<Assignment>(taskId);
                var project = _queryProcessor.FindById<Project>(task.ProjectId);
                var projectMembers = _queryProcessor.ExecuteQuery(new FindUserProfilesQuery
                {
                    UserIds = project.Members.Select(x => x.UserId)
                });

                var projectMembersViewModel = projectMembers.Select(x => new TaskAssigneeViewModel
                {
                    AssigneeId = x.UserId,
                    FullName = x.FullName
                });

                var model = _autoMapper.Map<EditAssignmentViewModel>(task);
                model.ProjectMembers = projectMembersViewModel;
                model.AssigneeIds = task.Assignees.Select(x => x.UserId).ToArray();

                return model;
            }
        }
    }
}