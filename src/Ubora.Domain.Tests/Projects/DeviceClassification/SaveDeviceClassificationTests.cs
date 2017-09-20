using Autofac;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.DeviceClassification;
using Ubora.Domain.Projects._Commands;
using Xunit;

namespace Ubora.Domain.Tests.Projects.DeviceClassification
{
    public class SaveDeviceClassificationTests : IntegrationFixture
    {
        protected override void RegisterAdditional(ContainerBuilder builder)
        {
            builder.RegisterType<TestDeviceClassificationProvider>().As<IDeviceClassificationProvider>().SingleInstance();
        }

        [Fact]
        public void Adds_Device_Classification_To_Project()
        {
            var processor = Container.Resolve<ICommandProcessor>();

            var expectedProjectId = Guid.NewGuid();
            processor.Execute(new CreateProjectCommand
            {
                Actor = new UserInfo(Guid.NewGuid(), ""),
                NewProjectId = expectedProjectId
            });

            var expectedUserInfo = new UserInfo(Guid.NewGuid(), "");

            var command = new SetDeviceClassificationForProjectCommand
            {
                DeviceClassification = new Classification("IIb", 3, new List<Guid>()),
                ProjectId = expectedProjectId,
                Actor = expectedUserInfo
            };

            // Act
            processor.Execute(command);

            // Assert
            var project = Session.Load<Project>(expectedProjectId);
            project.DeviceClassification.Should().Be("IIb");
        }

        [Fact]
        public void Updates_Device_Classification_On_Project_If_New_Classification_Is_Stronger()
        {
            var processor = Container.Resolve<ICommandProcessor>();

            var expectedProjectId = Guid.NewGuid();
            processor.Execute(new CreateProjectCommand
            {
                Actor = new UserInfo(Guid.NewGuid(), ""),
                NewProjectId = expectedProjectId
            });

            var command = new SetDeviceClassificationForProjectCommand
            {
                DeviceClassification = new Classification("IIb", 3, new List<Guid>()),
                ProjectId = expectedProjectId,
                Actor = new UserInfo(Guid.NewGuid(), "")
            };
            processor.Execute(command);

            var commandUpdate = new SetDeviceClassificationForProjectCommand
            {
                DeviceClassification = new Classification("III", 4, new List<Guid>()),
                ProjectId = expectedProjectId,
                Actor = new UserInfo(Guid.NewGuid(), "")
            };

            // Act
            processor.Execute(commandUpdate);

            // Assert
            var project = Session.Load<Project>(expectedProjectId);
            project.DeviceClassification.Should().Be("III");
        }

        [Fact]
        public void Throws_If_Project_Does_Not_Exist()
        {
            var processor = Container.Resolve<ICommandProcessor>();

            var expectedProjectId = Guid.NewGuid();

            var expectedUserInfo = new UserInfo(Guid.NewGuid(), "");

            var command = new SetDeviceClassificationForProjectCommand
            {
                DeviceClassification = new Classification("IIb", 3, new List<Guid>()),
                ProjectId = expectedProjectId,
                Actor = expectedUserInfo
            };

            // Act, Assert
            Assert.Throws<InvalidOperationException>(() => processor.Execute(command));
        }
    }

    internal class TestDeviceClassificationProvider : IDeviceClassificationProvider
    {
        public IDeviceClassification Provide()
        {
            var classification = new Domain.Projects.DeviceClassification.DeviceClassification();
            classification.CreateNew();

            return classification;
        }
    }
}
