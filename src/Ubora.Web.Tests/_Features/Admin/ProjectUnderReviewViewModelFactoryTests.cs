using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members;
using Ubora.Domain.Projects.Members.Queries;
using Ubora.Domain.Projects.Members.Specifications;
using Ubora.Domain.Users;
using Ubora.Web._Features.Admin;
using Xunit;

namespace Ubora.Web.Tests._Features.Admin
{
    public class ProjectUnderReviewViewModelFactoryTests
    {
        [Fact]
        public void Create_Returns_Expected_ViewModel()
        {
            var projectId = Guid.NewGuid();
            var title = "title";
           
            var projectMock = new Mock<Project>();
            projectMock.Object.Set(x => x.Id, projectId);
            projectMock.Object.Set(x => x.Title, title);

            var mentorId = Guid.NewGuid();
            var otherMentorId = Guid.NewGuid();
            var leaderId = Guid.NewGuid();
            var memberId = Guid.NewGuid();
            projectMock.Setup(x => x.GetMembers(new IsMentorSpec()))
                .Returns(new[] {
                    new ProjectMentor(mentorId),
                    new ProjectMentor(otherMentorId),
                });

            var mentors = new Dictionary<Guid, string>
            {
                {mentorId, "Mentor1"},
                {otherMentorId, "Mentor1"}
            };

            var mentorIds = new[] {mentorId, otherMentorId};
            var queryProcessorMock = new Mock<IQueryProcessor>();
            queryProcessorMock.Setup(x => x.ExecuteQuery(It.Is<FindFullNamesQuery>(q => q.UserIds == mentorIds)))
                .Returns(mentors);

            queryProcessorMock.Setup(p => p.ExecuteQuery(It.Is<FindFullNamesQuery>(q => AreEquivalent(q.UserIds, mentorIds))))
                .Returns(mentors);

            var factory = new ProjectUnderReviewViewModel.Factory(queryProcessorMock.Object);
            
            // Act
            var result = factory.Create(projectMock.Object);

            // Assert
            var expectedModel = new ProjectUnderReviewViewModel
            {
                Id = projectId,
                Title = title,
                ProjectMentors = mentors
            };

            result.ShouldBeEquivalentTo(expectedModel);
        }

        private bool AreEquivalent(IEnumerable<Guid> first, IEnumerable<Guid> second)
        {
            return first.OrderBy(f => f).SequenceEqual(second.OrderBy(s => s));
        }
    }
}
