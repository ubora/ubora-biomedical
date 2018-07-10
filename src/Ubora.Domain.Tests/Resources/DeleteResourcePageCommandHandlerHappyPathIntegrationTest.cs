using System;
using System.Linq;
using Autofac;
using FluentAssertions;
using Marten.Linq.SoftDeletes;
using Moq;
using Ubora.Domain.Infrastructure;
using Xunit;
using Ubora.Domain.Resources;
using Ubora.Domain.Resources.Commands;

namespace Ubora.Domain.Tests.Resources
{
    public class DeleteResourcePageCommandHandlerHappyPathIntegrationTest : IntegrationFixture
    {
        private readonly Mock<IResourceBlobDeleter> _resourceBlobDeleterMock = new Mock<IResourceBlobDeleter>();

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
                ResourcePageId = resourceId,
                Actor = new DummyUserInfo()
            });

            // Assert
            commandResult.IsSuccess.Should().BeTrue();

            Session.Load<ResourcePage>(resourceId)
                .Then(resourcePage =>
                {
                    resourcePage.IsDeleted().Should().BeTrue(); // Marten's
                });

            _resourceBlobDeleterMock
                .Verify(x => x.DeleteBlobContainerOfResourcePage(It.Is<ResourcePage>(page => page.Id == resourceId)), Times.Once);
        }
    }
}