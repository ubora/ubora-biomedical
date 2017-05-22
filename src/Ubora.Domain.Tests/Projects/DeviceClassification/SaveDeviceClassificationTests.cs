using Autofac;
using FluentAssertions;
using System;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Infrastructure.Marten;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.DeviceClassification;
using Xunit;

namespace Ubora.Domain.Tests.Projects.DeviceClassification
{
    public class SaveDeviceClassificationTests : IntegrationFixture
    {
        public SaveDeviceClassificationTests()
        {
            StoreOptions(new UboraStoreOptions().Configuration());
        }

        [Fact]
        public void Adds_Device_Classification_To_Project()
        {
            var processor = Container.Resolve<ICommandProcessor>();

            var expectedProjectId = Guid.NewGuid();
            processor.Execute(new CreateProjectCommand
            {
                UserInfo = new UserInfo(Guid.NewGuid(), ""),
                Id = expectedProjectId
            });

            var expectedUserInfo = new UserInfo(Guid.NewGuid(), "");

            var command = new SaveDeviceClassificationToProjectCommand
            {
                DeviceClassification = "IIb",
                ProjectId = expectedProjectId,
                Id = Guid.NewGuid(),
                UserInfo = expectedUserInfo
            };

            // Act
            processor.Execute(command);

            // Assert
            var project = Session.Load<Project>(expectedProjectId);
            project.DeviceClassification.Should().Be("IIb");
        }

        [Fact]
        public void Updates_Device_Classification_On_Project()
        {
            var processor = Container.Resolve<ICommandProcessor>();

            var expectedProjectId = Guid.NewGuid();
            processor.Execute(new CreateProjectCommand
            {
                UserInfo = new UserInfo(Guid.NewGuid(), ""),
                Id = expectedProjectId
            });

            var command = new SaveDeviceClassificationToProjectCommand
            {
                DeviceClassification = "IIb",
                ProjectId = expectedProjectId,
                Id = Guid.NewGuid(),
                UserInfo = new UserInfo(Guid.NewGuid(), "")
            };
            processor.Execute(command);

            var commandUpdate = new SaveDeviceClassificationToProjectCommand
            {
                DeviceClassification = "III",
                ProjectId = expectedProjectId,
                Id = Guid.NewGuid(),
                UserInfo = new UserInfo(Guid.NewGuid(), "")
            };

            // Act
            processor.Execute(commandUpdate);

            // Assert
            var project = Session.Load<Project>(expectedProjectId);
            project.DeviceClassification.Should().Be("III");
        }
    }
}
