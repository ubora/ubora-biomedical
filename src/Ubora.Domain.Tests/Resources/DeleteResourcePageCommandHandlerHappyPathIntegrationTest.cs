﻿using System;
using System.Linq;
using Autofac;
using FluentAssertions;
using Moq;
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
            var resourcePageId = Guid.NewGuid();
            new ResourcePageSeeder().WithId(resourcePageId).Seed(this);
            
            // Act
            var commandResult = Processor.Execute(new DeleteResourcePageCommand
            {
                ResourcePageId = resourcePageId,
                Actor = new DummyUserInfo()
            });

            // Assert
            commandResult.IsSuccess.Should().BeTrue();

            Session.Load<ResourcePage>(resourcePageId).Should().BeNull();

            _resourceBlobDeleterMock
                .Verify(x => x.DeleteBlobContainerOfResourcePage(It.Is<ResourcePage>(page => page.Id == resourcePageId)), Times.Once);

            // Link should be removed from menu
            Session.Load<ResourcesMenu>(ResourcesMenu.SingletonId)
                .Links.Any(link => link.Id == resourcePageId)
                .Should().BeFalse();
        }
    }
}