using System;
using FluentAssertions;
using TestStack.BDDfy;
using Ubora.Domain.Projects.Workpackages;
using Xunit;

namespace Ubora.Domain.Tests.Projects.Workpackages.Commands
{
    public class OpenWorkpackageThreeCommandHandlerTests : IntegrationFixture
    {
        private readonly Guid _projectId = Guid.NewGuid();

        [Fact]
        public void Workpackage_Three_Can_Be_Opened()
        {
            this.Given(_ => this.Create_Project(_projectId))
                .And(_ => this.Submit_Workpackage_One_For_Review(_projectId))
                .And(_ => this.Accept_Workpackage_One_Review(_projectId))
                .When(_ => this.Open_Workpackage_Three(_projectId))
                .Then(_ => Workpackage_Three_Should_Be_Available())
                .BDDfy();
        }

        protected void Workpackage_Three_Should_Be_Available()
        {
            var workpackageThree = Processor.FindById<WorkpackageThree>(_projectId);
            workpackageThree.Should().NotBeNull();
            workpackageThree.Steps.Count.Should().Be(12);
        }
    }
}
