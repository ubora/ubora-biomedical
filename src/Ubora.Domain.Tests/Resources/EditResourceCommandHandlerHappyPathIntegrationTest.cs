using System.Linq;
using FluentAssertions;
using Ubora.Domain.Resources;
using Ubora.Domain.Resources.Commands;
using Xunit;

namespace Ubora.Domain.Tests.Resources
{
    public class EditResourceCommandHandlerHappyPathIntegrationTest : IntegrationFixture
    {
        [Fact]
        public void Resource_Page_Content_Can_Be_Edited()
        {
            var initialContent = new QuillDelta("initialBody");
            var editedContent = new QuillDelta("editedBody");
            
            var resource = new ResourcePageSeeder()
                .WithTitle("initialTitle")
                .WithBody(initialContent)
                .Seed(this);
            
            // Act
            var commandResult = Processor.Execute(new EditResourcePageCommand
            {
                ResourcePageId = resource.Id,
                Title = "changedTitle",
                Body = editedContent,
                Actor = new DummyUserInfo(),
                PreviousContentVersion = resource.BodyVersion,
                MenuPriority = 321,
            });

            // Assert
            commandResult.IsSuccess.Should().BeTrue();

            var page = Session.Load<ResourcePage>(resource.Id);
            page.Body.Should().Be(editedContent);
            page.Title.Should().Be("changedTitle");
            page.MenuPriority.Should().Be(321);

            // Link should be changed in the menu
            var menuLink = Session.Load<ResourcesMenu>(ResourcesMenu.SingletonId).Links.Single(link => link.Id == resource.Id);
            menuLink.Title.Should().Be("changedTitle");
            menuLink.MenuPriority.Should().Be(321);
        }
    }
}