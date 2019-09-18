using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects._Specifications;
using Ubora.Domain.Projects.Assignments;

namespace Ubora.Web._Features.Projects.Assignments
{
    public class AssignmentListViewModel
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public IEnumerable<AssignmentListItemViewModel> Assignments { get; set; }
        public bool CanWorkOnAssignments { get; set; }

        public class Factory
        {
            private readonly IQueryProcessor _queryProcessor;
            private readonly IAuthorizationService _authorizationService;

            public Factory(IQueryProcessor queryProcessor, IAuthorizationService authorizationService)
            {
                _queryProcessor = queryProcessor;
                _authorizationService = authorizationService;
            }

            public async Task<AssignmentListViewModel> Create(ClaimsPrincipal user, Project project)
            {
                var projectTasks = _queryProcessor.Find<Assignment>(
                    new IsFromProjectSpec<Assignment>
                    {
                        ProjectId = project.Id
                    });

                var canWorkOnAssignments = 
                    (await _authorizationService.AuthorizeAsync(user, null, Policies.CanWorkOnProjectContent)).Succeeded;

                var assignments = projectTasks.Select(a => new AssignmentListItemViewModel
                (
                    projectId: a.ProjectId,
                    id: a.Id,
                    createdByUserId: a.CreatedByUserId,
                    title: a.Title,
                    description: a.Description,
                    isDone: a.IsDone
                ));

                return new AssignmentListViewModel
                {
                    ProjectId = project.Id,
                    ProjectName = project.Title,
                    Assignments = assignments.OrderBy(a => a.Title),
                    CanWorkOnAssignments = canWorkOnAssignments
                };
            }
        }
    }

    public class AssignmentListItemViewModel
    {
        public Guid ProjectId { get; set; }
        public Guid Id { get; set; }
        public Guid CreatedByUserId { get; private set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; set; }

        public AssignmentListItemViewModel()
        {
        }

        public AssignmentListItemViewModel(Guid projectId, Guid id, Guid createdByUserId, string title, string description, bool isDone)
        {
            ProjectId = projectId;
            Id = id;
            CreatedByUserId = createdByUserId;
            Title = title;
            Description = description;
            IsDone = isDone;
        }
    }
}