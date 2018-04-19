using System;
using System.Linq;
using FluentAssertions;
using Marten.Linq.SoftDeletes;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Resources;
using Xunit;

namespace Ubora.Domain.Tests.Resources
{
    public class DeleteResourceCommandHandlerHappyPathIntegrationTest : IntegrationFixture
    {
        [Fact]
        public void Resource_Page_Can_Be_Deleted()
        {
            var resourceId = Guid.NewGuid();
            new ResourceBuilder().WithId(resourceId).Build(this);
            
            // Act
            var commandResult = Processor.Execute(new DeleteResourceCommand
            {
                ResourceId = resourceId,
                Actor = new DummyUserInfo()
            });

            // Assert
            commandResult.IsSuccess.Should().BeTrue();

            var lastEventInStream = Session.Events.FetchStream(resourceId).Select(e => e.Data).ToList().Last();
            lastEventInStream.Should().BeOfType<ResourceDeletedEvent>();

            Session.Load<Resource>(resourceId)
                .Then(resourcePage =>
                {
                    resourcePage.IsDeleted().Should().BeTrue(); // Marten's
                    resourcePage.IsDeleted.Should().BeTrue();
                });
        }
    }
}