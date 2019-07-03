using System;
using System.Linq;
using FluentAssertions;
using Ubora.Domain.Resources;
using Ubora.Domain.Resources.Commands;
using Ubora.Domain.Resources.Events;
using Xunit;

namespace Ubora.Domain.Tests.Resources
{
    public class CreateResourcePageCommandHandlerHappyPathInterationTest : IntegrationFixture
    {
        [Fact]
        public void Resource_Page_Can_Be_Created()
        {
            var resourceId = Guid.NewGuid();
            var command = new CreateResourcePageCommand
            {
                ResourcePageId = resourceId,
                Title = "Introduction page",
                Body = new QuillDelta("{Hello, and welcome!}"),
                Actor = new DummyUserInfo(),
                MenuPriority = 123,
                ParentCategoryId = null
            };
            
            // Act
            var commandResult = Processor.Execute(command);

            // Assert
            commandResult.IsSuccess.Should().BeTrue();

            var singleEventInStream = Session.Events.FetchStream(resourceId).ToList().Single();
            singleEventInStream.Data.Should().BeOfType<ResourcePageCreatedEvent>();
            
            var resourcePage = Session.Load<ResourcePage>(resourceId);

            resourcePage.Id.Should().Be(resourceId);
            resourcePage.Title.Should().Be("Introduction page");
            resourcePage.Body.Should().Be(new QuillDelta("{Hello, and welcome!}"));
            resourcePage.MenuPriority.Should().Be(123);
            resourcePage.CategoryId.Should().BeNull();

            // Link should be in menu
            Session.Load<ResourcesMenu>(ResourcesMenu.SingletonId)
                .Links.OfType<ResourcePageLink>()
                .Any(link => link.Id == resourceId && link.Title == "Introduction page")
                .Should().BeTrue();
        }
    }
}