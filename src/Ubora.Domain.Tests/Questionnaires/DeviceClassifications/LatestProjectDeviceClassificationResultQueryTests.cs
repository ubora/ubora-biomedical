using System;
using FluentAssertions;
using Marten;
using Marten.Linq;
using Moq;
using Ubora.Domain.Questionnaires.DeviceClassifications;
using Ubora.Domain.Questionnaires.DeviceClassifications.Queries;
using Xunit;

namespace Ubora.Domain.Tests.Questionnaires.DeviceClassifications
{
    public class LatestProjectDeviceClassificationResultQueryIntegrationTests : IntegrationFixture
    {
        [Fact]
        public void Returns_Latest_Finished_Device_Classification()
        {
            var projectId = Guid.NewGuid();
            var expectedDeviceClassification = 
                CreateDeviceClassification(projectId: projectId, finishedAt: DateTime.UtcNow.AddHours(1));

            var deviceClassifications = new []
            {
                CreateDeviceClassification(projectId: Guid.NewGuid()),
                CreateDeviceClassification(projectId: Guid.NewGuid(), finishedAt: DateTime.UtcNow.AddHours(-1)),
                CreateDeviceClassification(projectId: Guid.NewGuid(), finishedAt: DateTime.UtcNow.AddHours(2)),
                CreateDeviceClassification(projectId: projectId),
                CreateDeviceClassification(projectId: projectId, finishedAt: DateTime.UtcNow),
                expectedDeviceClassification
            };

            Session.StoreObjects(deviceClassifications);
            Session.SaveChanges();

            var query = new LatestFinishedProjectDeviceClassificationQuery(projectId);

            // Act
            var result = Processor.ExecuteQuery(query);

            // Assert
            result.Id.Should().Be(expectedDeviceClassification.Id);
        }

        private static DeviceClassificationAggregate CreateDeviceClassification(Guid projectId, DateTime? finishedAt = null)
        {
            return new DeviceClassificationAggregate()
                .Set(x => x.Id, Guid.NewGuid())
                .Set(x => x.ProjectId, projectId)
                .Set(x => x.FinishedAt, finishedAt);
        }
    }
}
