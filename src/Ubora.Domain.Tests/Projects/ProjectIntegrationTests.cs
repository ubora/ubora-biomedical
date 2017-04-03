using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Ubora.Domain.Events;
using Ubora.Domain.Projects.Events;
using Ubora.Domain.Projects.Projections;
using Xunit;

namespace Ubora.Domain.Tests.Projects
{
    public class ProjectIntegrationTests : DocumentSessionIntegrationFixture
    {
        public ProjectIntegrationTests()
        {
           StoreOptions(new UboraStoreOptions().Configuration());  
        }

        [Fact]
        public void WorkPackageCreated_event_is_projected_as_a_new_Workpackage_doc()
        {
            var projectId = Guid.NewGuid();
            var user = new UserInfo(Guid.NewGuid(), "Test User");

            // Act
            Session.Events.StartStream<Project>(projectId,
                new ProjectCreated("My test project", user),
                new WorkpackageCreated("WP 1", user),
                new WorkpackageCreated("WP 2", user));
            Session.SaveChanges();

            // Assert
            var workpackages = Session.Query<Workpackage>().ToList();
            workpackages.Count().Should().Be(2);
            workpackages.Should().Contain(w => w.Name == "WP 1" && w.ProjectId == projectId);
            workpackages.Should().Contain(w => w.Name == "WP 2" && w.ProjectId == projectId);
        }
    }
}
