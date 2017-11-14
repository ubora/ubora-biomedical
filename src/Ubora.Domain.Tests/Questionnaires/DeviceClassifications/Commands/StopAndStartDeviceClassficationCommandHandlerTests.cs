using System;
using FluentAssertions;
using Marten;
using Moq;
using Ubora.Domain.Questionnaires.DeviceClassifications;
using Ubora.Domain.Questionnaires.DeviceClassifications.Commands;
using Xunit;

namespace Ubora.Domain.Tests.Questionnaires.DeviceClassifications.Commands
{
    public class StopAndStartDeviceClassficationCommandHandlerTests
    {
        [Fact]
        public void Handle_Returns_Error_When_Old_Questionnaire_Is_Already_Stopped()
        {
            var documentSessionMock = new Mock<IDocumentSession>(MockBehavior.Strict);

            var command = new StopAndStartDeviceClassificationCommand
            {
                StopQuestionnaireId = Guid.NewGuid()
            };

            var deviceClassificationAggregate = new DeviceClassificationAggregate()
                .Set(x => x.FinishedAt, DateTime.UtcNow);

            documentSessionMock.Setup(x => x.Load<DeviceClassificationAggregate>(command.StopQuestionnaireId))
                .Returns(deviceClassificationAggregate);

            var handler = new StopAndStartDeviceClassificationCommand.Handler(documentSessionMock.Object, Mock.Of<DeviceClassificationQuestionnaireTreeFactory>());

            // Act
            var result = handler.Handle(command);

            // Assert
            result.IsSuccess.Should().BeFalse();
        }
    }
}