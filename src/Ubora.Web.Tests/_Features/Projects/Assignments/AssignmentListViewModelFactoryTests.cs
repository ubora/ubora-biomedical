using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects._Specifications;
using Ubora.Domain.Projects.Assignments;
using Ubora.Web._Features.Projects.Assignments;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.Assignments
{
    public class AssignmentListViewModelFactoryTests
    {
        private readonly Mock<IQueryProcessor> _queryProcessorMock;
        private readonly Mock<IAuthorizationService> _authorizationService;
        private readonly AssignmentListViewModel.Factory _factory;

        public AssignmentListViewModelFactoryTests()
        {
            _queryProcessorMock = new Mock<IQueryProcessor>();
            _authorizationService = new Mock<IAuthorizationService>();
            _factory = new AssignmentListViewModel.Factory(_queryProcessorMock.Object, _authorizationService.Object);
        }

        [Fact]
        public async Task Create_Returns_Expected_ViewModel()
        {
            var projectId = new Guid();
            var projectTitle = "Project test title";
            var project = Mock.Of<Project>();
            project.Set(p => p.Id, projectId);
            project.Set(p => p.Title, projectTitle);

            var user = Mock.Of<ClaimsPrincipal>();
            _authorizationService
                    .Setup(a => a.AuthorizeAsync(user, null, Policies.CanWorkOnAssignments))
                    .ReturnsAsync(AuthorizationResult.Success());

            var user1Id = Guid.NewGuid();
            var user2Id = Guid.NewGuid();

            var assignmentId = Guid.NewGuid();

            var assignment = Mock.Of<Assignment>();
            assignment.Set(a => a.Id, assignmentId)
                        .Set(a => a.ProjectId, projectId)
                        .Set(a => a.CreatedByUserId, user1Id)
                        .Set(a => a.Title, "Assignment 1")
                        .Set(a => a.Description, "Description 1")
                        .Set(a => a.IsDone, true)
                        .Set(a => a.Assignees, new List<TaskAssignee> { new TaskAssignee(user1Id) });

            _queryProcessorMock.Setup(p => p.Find<Assignment>(new IsFromProjectSpec<Assignment> { ProjectId = projectId }))
                        .Returns(new PagedList<Assignment>(new[] { assignment }, new Paging(1, int.MaxValue), 2));

            var expectedViewModel = new AssignmentListViewModel
            {
                ProjectId = projectId,
                ProjectName = projectTitle,
                Assignments = new []
                { 
                    new AssignmentListItemViewModel
                    (
                        projectId: projectId,
                        id: assignmentId,
                        createdByUserId: user1Id,
                        title: "Assignment 1",
                        description: "Description 1",
                        isDone: true
                    )
                },
                CanWorkOnAssignments = true
            };

            // Act
            var result = await _factory.Create(user, project);

            // Assert
            result.ShouldBeEquivalentTo(expectedViewModel);
        }
    }
}
