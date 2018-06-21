using System;
using System.Linq;
using Autofac;
using FluentAssertions;
using Marten.Linq.SoftDeletes;
using Moq;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Resources;
using Ubora.Domain.Resources.Commands;
using Ubora.Domain.Resources.Events;
using Xunit;

namespace Ubora.Domain.Tests.Resources
{
    public class DeleteResourcePageCommandHandlerHappyPathIntegrationTest : IntegrationFixture
    {
        private Mock<IResourceBlobDeleter> _resourceBlobDeleterMock = new Mock<IResourceBlobDeleter>();

        protected override void RegisterAdditional(ContainerBuilder builder)
        {
            builder.RegisterInstance(_resourceBlobDeleterMock.Object);
        }

        [Fact]
        public void Resource_Page_Can_Be_Deleted()
        {
            var resourceId = Guid.NewGuid();
            new ResourcePageBuilder().WithId(resourceId).Build(this);
            
            // Act
            var commandResult = Processor.Execute(new DeleteResourcePageCommand
            {
                ResourceId = resourceId,
                Actor = new DummyUserInfo()
            });

            // Assert
            commandResult.IsSuccess.Should().BeTrue();

            var lastEventInStream = Session.Events.FetchStream(resourceId).Select(e => e.Data).ToList().Last();
            lastEventInStream.Should().BeOfType<ResourcePageDeletedEvent>();

            Session.Load<ResourcePage>(resourceId)
                .Then(resourcePage =>
                {
                    resourcePage.IsDeleted().Should().BeTrue(); // Marten's
                    resourcePage.IsDeleted.Should().BeTrue();
                });

            _resourceBlobDeleterMock
                .Verify(x => x.DeleteBlobContainerOfResourcePage(It.Is<ResourcePage>(page => page.Id == resourceId)), Times.Once);
        }
    }
}