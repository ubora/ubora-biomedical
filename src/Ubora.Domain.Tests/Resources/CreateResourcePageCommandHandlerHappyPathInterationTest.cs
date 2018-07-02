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
                Body = new QuillDelta("Hello, and welcome!"),
                Actor = new DummyUserInfo(),
                MenuPriority = 123
            };
            
            // Act
            var commandResult = Processor.Execute(command);

            // Assert
            commandResult.IsSuccess.Should().BeTrue();

            var singleEventInStream = Session.Events.FetchStream(resourceId).ToList().Single();
            singleEventInStream.Data.Should().BeOfType<ResourcePageCreatedEvent>();
            
            var resource = Session.Load<ResourcePage>(resourceId);

            resource.Id.Should().Be(resourceId);
            resource.Title.Should().Be("Introduction page");
            resource.Body.Should().Be(new QuillDelta("Hello, and welcome!"));
            resource.ActiveSlug.Value.Should().Be("introduction-page");
            resource.MenuPriority.Should().Be(123);
        }
    }
}