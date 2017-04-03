using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Baseline;
using FluentAssertions;
using Ubora.Domain.Events;
using Ubora.Domain.Projects.Events;
using Ubora.Domain.Projects.Projections;
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
                new ProjectCreated("My first project", new UserInfo(Guid.NewGuid(), "Mari Pari")),
                new WorkpackageCreated("Project initialization", new UserInfo(Guid.NewGuid(), "Karl Parl")));
            var project2Id = Guid.NewGuid();
            Session.Events.Append(project2Id,
                new ProjectCreated("My Second project", new UserInfo(Guid.NewGuid(), "Eeri Peeri")),
                new WorkpackageCreated("Project initialization", new UserInfo(Guid.NewGuid(), "Lüri Püri")));
            Session.SaveChanges();
            Session.Events.Append(project1Id, 4, 
                new WorkpackageCreated("Project planning", new UserInfo(Guid.NewGuid(), "Mari Pari")),
                new ProjectRenamed("My first and only project", new UserInfo(Guid.NewGuid(), "Mari Pari")));
            Session.SaveChanges();

            Session.Events.QueryAllRawEvents().Count().Should().Be(6);
            Session.Events.FetchStream(project1Id).Each(e => Debug.WriteLine(e.Data.ToString()));

        }
    }
}
