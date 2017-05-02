using System;
using System.Diagnostics;
using System.Linq;
using Baseline;
using FluentAssertions;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects;
using Xunit;

namespace Ubora.Domain.Tests
{
    public class Playground : DocumentSessionIntegrationFixture
    {
        public Playground()
        {
            StoreOptions(new UboraStoreOptions().Configuration());
        }

        [Fact]
        public void Events_Are_Queriable()
        {
            var project1Id = Guid.NewGuid();

            Session.Events.Append(project1Id,
                new ProjectCreatedEvent(new UserInfo(Guid.NewGuid(), "")) { Title = "My first project"}
               /* new WorkpackageCreatedEvent("Project initialization", new UserInfo(Guid.NewGuid(), "Karl Parl"))*/);
            var project2Id = Guid.NewGuid();

            //Session.Events.Append(project2Id,
            //    new ProjectCreatedEvent(new UserInfo(Guid.NewGuid(), "Eeri Peeri")) { Title = "My second project" }
            //    /*new WorkpackageCreatedEvent("Project initialization", new UserInfo(Guid.NewGuid(), "Lüri Püri"))*/);
            Session.SaveChanges();

            //Session.Events.Append(project1Id, 3,
            //    new WorkpackageCreatedEvent("Project planning", new UserInfo(Guid.NewGuid(), "Mari Pari")));
            //Session.SaveChanges();

            Session.Events.QueryAllRawEvents().Count().Should().Be(2);
            Session.Events.FetchStream(project1Id).Each(e => Debug.WriteLine(e.Data.ToString()));

            var project1 = Session.Load<Project>(project1Id);
            var project2 = Session.Load<Project>(project2Id);

            project1.Should().NotBeNull();
            project2.Should().NotBeNull();
        }
    }
}
