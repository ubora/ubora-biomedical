using System.Linq;
using FluentAssertions;
using Ubora.Domain.Resources;
using Ubora.Domain.Resources.Commands;
using Xunit;

namespace Ubora.Domain.Tests.Resources
{
    public class EditResourceContentCommandHandlerHappyPathIntegrationTest : IntegrationFixture
    {
        [Fact]
        public void Resource_Page_Content_Can_Be_Edited()
        {
            var initialContent = new QuillDelta("initialBody");
            var editedContent = new QuillDelta("editedBody");
            
            var resource = new ResourcePageBuilder()
                .WithTitle("initialTitle")
                .WithBody(initialContent)
                .Build(this);
            
            // Act
            var commandResult = Processor.Execute(new EditResourcePageContentCommand
            {
                ResourceId = resource.Id,
                Title = "changedTitle",
                Body = editedContent,
                Actor = new DummyUserInfo(),
                PreviousContentVersion = resource.BodyVersion
            });

            // Assert
            commandResult.IsSuccess.Should().BeTrue();

            var page = Session.Load<ResourcePage>(resource.Id);
            page.Body.Should().Be(editedContent);
            page.Title.Should().Be("changedTitle");
        }
    }
}