using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Marten;
using Ubora.Domain.Infrastructure.Marten;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.History;
using Ubora.Domain.Projects._Specifications;
using Xunit;

namespace Ubora.Domain.Tests.Projects._Specifications
{
    public class IsFromProjectSpecTests : DocumentSessionIntegrationFixture
    {
        public IsFromProjectSpecTests()
        {
            StoreOptions(o =>
            {
                new UboraStoreOptionsConfigurer().CreateConfigureAction(new List<Type>(), new List<MappedType>(),
                    AutoCreate.CreateOnly).Invoke(o);
            });
        }

        [Fact]
        public void Must_Filter_Projects_By_ProjectId()
        {
            var expectedProjectId = Guid.NewGuid();
            Session.Insert(new TestProjecEntity());
            Session.Insert(new TestProjecEntity { ProjectId = expectedProjectId});
            Session.Insert(new TestProjecEntity());
            Session.SaveChanges();
            RefreshSession();

            // Act
            var satisfiedProject = new IsFromProjectSpec<TestProjecEntity>() { ProjectId = expectedProjectId }
                .SatisfyEntitiesFrom(Session.Query<TestProjecEntity>()).Single();

            // Assert
            satisfiedProject.ProjectId.Should().Be(expectedProjectId);
        }
    }

    public class TestProjecEntity : IProjectEntity
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
    }

   
}
