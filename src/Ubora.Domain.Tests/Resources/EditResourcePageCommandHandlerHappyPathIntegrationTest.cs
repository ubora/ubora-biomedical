using System;
using System.Linq;
using FluentAssertions;
using Ubora.Domain.Resources;
using Ubora.Domain.Resources.Commands;
using Xunit;

namespace Ubora.Domain.Tests.Resources
{
    public class EditResourcePageCommandHandlerHappyPathIntegrationTest : IntegrationFixture
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

            var changedParentCategoryId = Guid.NewGuid();

            // Act
            var commandResult = Processor.Execute(new EditResourcePageCommand
            {
                ResourcePageId = resource.Id,
                Title = "changedTitle",
                Body = editedContent,
                Actor = new DummyUserInfo(),
                PreviousContentVersion = resource.BodyVersion,
                MenuPriority = 200,
                ParentCategoryId = changedParentCategoryId
            });

            // Assert
            commandResult.IsSuccess.Should().BeTrue();

            var page = Session.Load<ResourcePage>(resource.Id);
            page.Body.Should().Be(editedContent);
            page.Title.Should().Be("changedTitle");
            page.MenuPriority.Should().Be(200);
            page.CategoryId.Should().Be(changedParentCategoryId);

            // Link should be changed in the menu
            var menuLink = Session.Load<ResourcesMenu>(ResourcesMenu.SingletonId).Links.Single(link => link.Id == resource.Id);
            menuLink.Title.Should().Be("changedTitle");
            menuLink.MenuPriority.Should().Be(200);
            menuLink.ParentCategoryId.Should().Be(changedParentCategoryId);
        }
    }
}