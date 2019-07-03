using FluentAssertions;
using System.Linq;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects._Commands;
using Ubora.Domain.Projects._Events;
using Xunit;

namespace Ubora.Domain.Tests.Projects._Commands
{
    public class ChangeAgreementToTermsOfUboraCommandTests : IntegrationFixture
    {
        [Fact]
        public void Change_Agreement_Of_UBORA_Terms_Feature_Is_Implemented()
        {
            var actor = new DummyUserInfo();

            // Execute cases
            Case_From_Disagreement_To_Agreement();
            Case_From_Agreement_To_Disagreement();
            Case_Not_Allowed_When_WP5_Locked();

            // # Cases
            void Case_From_Disagreement_To_Agreement()
            {
                var project = new ProjectSeeder()
                    .WithWp1Accepted()
                    .WithWp3Unlocked()
                    .WithWp4Unlocked()
                    .WithWp5Unlocked()
                    .Seed(this);

                var command = new ChangeAgreementToTermsOfUboraCommand
                {
                    IsAgreed = true,
                    Actor = actor,
                    ProjectId = project.Id
                };

                // Assume
                project.IsAgreedToTermsOfUbora.Should().BeFalse();

                // Act
                var result1 = Processor.Execute(command);
                var result2 = Processor.Execute(command);

                // Assert
                result1.IsSuccess.Should().BeTrue();
                result2.IsSuccess.Should().BeTrue();
                project = Session.Load<Project>(project.Id);
                project.IsAgreedToTermsOfUbora.Should().BeTrue();
                Session.Events.FetchStream(project.Id).Select(x => x.Data).OfType<AgreementWithTermsOfUboraChangedEvent>().ToList().Should().HaveCount(1);
                var @event = (AgreementWithTermsOfUboraChangedEvent)Session.Events.FetchStream(project.Id).Select(x => x.Data).ToList().Last();
                @event.InitiatedBy.Should().Be(actor);
            }

            void Case_From_Agreement_To_Disagreement()
            {
                var project = new ProjectSeeder()
                    .WithWp1Accepted()
                    .WithWp3Unlocked()
                    .WithWp4Unlocked()
                    .WithWp5Unlocked()
                    .WithAgreementToTermsOfUbora(true)
                    .Seed(this);

                var command = new ChangeAgreementToTermsOfUboraCommand
                {
                    IsAgreed = true,
                    Actor = actor,
                    ProjectId = project.Id
                };

                // Act
                var result1 = Processor.Execute(command);
                var result2 = Processor.Execute(command);

                // Assert
                result1.IsSuccess.Should().BeTrue();
                result2.IsSuccess.Should().BeTrue();
                project = Session.Load<Project>(project.Id);
                project.IsAgreedToTermsOfUbora.Should().BeTrue();
            }

            void Case_Not_Allowed_When_WP5_Locked()
            {
                var project = new ProjectSeeder()
                    .WithWp1Accepted()
                    .WithWp3Unlocked()
                    .WithWp4Unlocked()
                    .Seed(this);

                var command = new ChangeAgreementToTermsOfUboraCommand
                {
                    IsAgreed = true,
                    Actor = actor,
                    ProjectId = project.Id
                };

                // Act
                var result = Processor.Execute(command);

                // Assert
                result.IsSuccess.Should().BeFalse();
            }
        }
    }
}
