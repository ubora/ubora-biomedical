using System;
using System.Linq;
using FluentAssertions;
using Ubora.Domain.Resources;
using Xunit;

namespace Ubora.Domain.Tests.Resources
{
    public class CreateResourceCommandHandlerHappyPathInterationTest : IntegrationFixture
    {
        [Fact]
        public void Resource_Page_Can_Be_Created()
        {
            var resourceId = Guid.NewGuid();
            var content = new ResourceContent(
                title: "Introduction page",
                body: "Hello, and welcome!");
            
            var command = new CreateResourceCommand
            {
                ResourceId = resourceId,
                Content = content,
                Actor = new DummyUserInfo()
            };
            
            // Act
            var commandResult = Processor.Execute(command);

            // Assert
            commandResult.IsSuccess.Should().BeTrue();

            var singleEventInStream = Session.Events.FetchStream(resourceId).ToList().Single();
            singleEventInStream.Data.Should().BeOfType<ResourceCreatedEvent>();
            
            var resource = Session.Load<Resource>(resourceId);

            resource.Id.Should().Be(resourceId);
            resource.Content.ShouldBeEquivalentTo(content);
            resource.Slug.Value.Should().Be("introduction-page");
        }
    }
}