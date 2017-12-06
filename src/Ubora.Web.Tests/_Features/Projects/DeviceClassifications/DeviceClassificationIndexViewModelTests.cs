using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Moq;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects._Specifications;
using Ubora.Domain.Questionnaires.DeviceClassifications;
using Ubora.Web._Features.Projects.DeviceClassifications;
using Xunit;
using QuestionnaireListItem = Ubora.Web._Features.Projects.DeviceClassifications.DeviceClassificationIndexViewModel.QuestionnaireListItem;

namespace Ubora.Web.Tests._Features.Projects.DeviceClassifications
{
    public class DeviceClassificationIndexViewModelTests
    {
        [Fact]
        public void Factorr_Should_Return_All_Non_Stopped_Project_Classifications()
        {
            var queryProcessorMock = new Mock<IQueryProcessor>();
            var projectId = Guid.NewGuid();
            var lastNonStoppedClassification = new QuestionnaireListItem { IsStopped = false, StartedAt = DateTime.Now.AddMinutes(3) };
            var firstPreviousClassification = new QuestionnaireListItem { IsStopped = false, StartedAt = DateTime.Now.AddMinutes(2) };
            var secondPreviousClassification = new QuestionnaireListItem { IsStopped = false, StartedAt = DateTime.Now.AddMinutes(1) };
            var classifications = new PagedListStub<QuestionnaireListItem>
            {
                new QuestionnaireListItem{ IsStopped = true, StartedAt = DateTime.Now.AddMinutes(-1)},
                secondPreviousClassification,
                lastNonStoppedClassification,
                firstPreviousClassification
            };
            queryProcessorMock
                .Setup(q => q.Find(
                    new IsFromProjectSpec<DeviceClassificationAggregate> {ProjectId = projectId}, 
                    It.IsAny<DeviceClassificationIndexViewModel.QuestionnaireListItemProjection>(),
                    null,
                    int.MaxValue, 1))
                .Returns(classifications);
            var viewModelFactory = new DeviceClassificationIndexViewModel.Factory(queryProcessorMock.Object);

            // Act
            var model = viewModelFactory.Create(projectId);

            // Assert
            model.Previous.Should().Equal(firstPreviousClassification, secondPreviousClassification);
            model.Last.Should().BeSameAs(lastNonStoppedClassification);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void QuestionnaireListItemProjection_Maps_To_ListItem(bool stopped, bool finished)
        {
            var classification = Mock.Of<DeviceClassificationAggregate>(d => d.IsStopped == stopped)
                .Set(d => d.FinishedAt, finished ? (DateTime?)DateTime.UtcNow : null)
                .Set(d => d.StartedAt, DateTime.UtcNow)
                .Set(d => d.Id, Guid.NewGuid());

            var projection = new DeviceClassificationIndexViewModel.QuestionnaireListItemProjection();
            // Act
            var projectedModel = projection.Apply(new[] {classification}.AsQueryable()).Single();
            // Assert
            projectedModel.ShouldBeEquivalentTo(new QuestionnaireListItem
            {
                IsStopped  =  stopped,
                StartedAt = classification.StartedAt,
                IsFinished = finished,
                QuestionnaireId = classification.Id
            } );
        }
    }
}
