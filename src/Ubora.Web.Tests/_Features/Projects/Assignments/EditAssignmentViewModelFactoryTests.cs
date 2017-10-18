using AutoMapper;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members;
using Ubora.Domain.Projects.Members.Queries;
using Ubora.Domain.Projects.Tasks;
using Ubora.Domain.Users;
using Ubora.Web._Features.Projects.Assignments;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.Assignments
{
    public class EditAssignmentViewModelFactoryTests
    {
        private readonly Mock<IQueryProcessor> _queryProcessorMock;
        private readonly Mock<IMapper> _autoMapperMock;
        private readonly EditAssignmentViewModel.Factory _factory;

        public EditAssignmentViewModelFactoryTests()
        {
            _queryProcessorMock = new Mock<IQueryProcessor>();
            _autoMapperMock = new Mock<IMapper>();
            _factory = new EditAssignmentViewModel.Factory(_queryProcessorMock.Object, _autoMapperMock.Object);
        }

        [Fact]
        public void Create_Returns_Expected_ViewModel()
        {
            var projectId = Guid.NewGuid();
            var project = Mock.Of<Project>();
            project.Set(p => p.Id, projectId);

            var user1Id = Guid.NewGuid();
            var user2Id = Guid.NewGuid();
            var user3Id = Guid.NewGuid();
            var projectMembers = new List<ProjectMember>
            {
                new ProjectLeader(user1Id),
                new ProjectMember(user2Id),
                new ProjectMember(user3Id)
            };

            Mock.Get(project).Setup(x => x.Members)
                .Returns(projectMembers);

            _queryProcessorMock.Setup(p => p.FindById<Project>(projectId))
                .Returns(project);

            var taskId = Guid.NewGuid();
            var task = Mock.Of<ProjectTask>();
            task.Set(t => t.Id, taskId);
            task.Set(t => t.ProjectId, projectId);
            task.Set(t => t.Title, "Title");
            task.Set(t => t.Description, "Description");
            Mock.Get(task).Setup(x => x.Assignees)
                .Returns(new[] { new TaskAssignee(user1Id), new TaskAssignee(user3Id) });
            //task.Set(t => t.Assignees, new[] { new TaskAssignee(user1Id), new TaskAssignee(user3Id) });

            _queryProcessorMock.Setup(p => p.FindById<ProjectTask>(taskId))
                .Returns(task);

            var userProfiles = new[]
            {
                new UserProfile(user1Id).Set(x => x.FirstName, "User1"),
                new UserProfile(user2Id).Set(x => x.FirstName, "User2"),
                new UserProfile(user3Id).Set(x => x.FirstName, "User3")
            };

            var projectMemberIds = projectMembers.Select(x => x.UserId);
            _queryProcessorMock.Setup(x => x.ExecuteQuery(It.Is<FindUserProfilesQuery>(q => q.UserIds == projectMemberIds)))
                .Returns(userProfiles);

            _queryProcessorMock.Setup(p => p.ExecuteQuery(It.Is<FindUserProfilesQuery>(q => AreEquivalent(q.UserIds, projectMemberIds))))
                .Returns(userProfiles);

            var taskAssigneeViewModel = new[]
            {
                new TaskAssigneeViewModel{AssigneeId = user1Id, FullName = "User1 "},
                new TaskAssigneeViewModel{AssigneeId = user2Id, FullName = "User2 "},
                new TaskAssigneeViewModel{AssigneeId = user3Id, FullName = "User3 "}
            };

            var autoMappedModel = new EditAssignmentViewModel
            {
                Id = task.Id,
                ProjectId = projectId,
                Title = task.Title,
                Description = task.Description
            };

            _autoMapperMock.Setup(m => m.Map<EditAssignmentViewModel>(task))
                .Returns(autoMappedModel);

            // Act
            var result = _factory.Create(taskId);

            // Assert
            var expectedModel = new EditAssignmentViewModel
            {
                Id = task.Id,
                ProjectId = projectId,
                Title = task.Title,
                Description = task.Description,
                AssigneeIds = new[] { user1Id, user3Id },
                ProjectMembers = taskAssigneeViewModel
            };

            result.ShouldBeEquivalentTo(expectedModel);
        }

        private bool AreEquivalent(IEnumerable<Guid> first, IEnumerable<Guid> second)
        {
            return Enumerable.SequenceEqual(first.OrderBy(f => f), second.OrderBy(s => s));
        }
    }
}
