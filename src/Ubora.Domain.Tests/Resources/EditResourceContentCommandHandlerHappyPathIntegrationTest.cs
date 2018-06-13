using System.Linq;
using FluentAssertions;
using Ubora.Domain.Resources;
using Ubora.Domain.Resources.Commands;
using Ubora.Domain.Resources.Events;
using Xunit;

namespace Ubora.Domain.Tests.Resources
{
    public class EditResourceContentCommandHandlerHappyPathIntegrationTest : IntegrationFixture
    {
        [Fact]
        public void Resource_Page_Content_Can_Be_Edited()
        {
            var initialContent = new ResourceContent("initialTitle", "initialBody");
            var editedContent = new ResourceContent("editedTitle", "editedBody");
            
            var resource = new ResourcePageBuilder()
                .WithContent(initialContent)
                .Build(this);
            
            // Act
            var commandResult = Processor.Execute(new EditResourceContentCommand
            {
                ResourceId = resource.Id,
                Content = editedContent,
                Actor = new DummyUserInfo(),
                PreviousContentVersion = resource.ContentVersion
            });

            // Assert
            commandResult.IsSuccess.Should().BeTrue();

            var lastEventInStream = Session.Events.FetchStream(resource.Id).Select(e => e.Data).ToList().Last();
            lastEventInStream.Should().BeOfType<ResourceContentEditedEvent>();

            Session.Load<ResourcePage>(resource.Id)
                .Content.ShouldBeEquivalentTo(editedContent);
        }
    }
}