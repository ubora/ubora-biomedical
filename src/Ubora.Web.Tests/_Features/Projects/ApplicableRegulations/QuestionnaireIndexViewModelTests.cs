using System;
using System.Linq;
using FluentAssertions;
using Moq;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects._Specifications;
using Ubora.Domain.Questionnaires.ApplicableRegulations;
using Ubora.Web._Features.Projects.ApplicableRegulations;
using Xunit;
using QuestionnaireListItem = Ubora.Web._Features.Projects.ApplicableRegulations.QuestionnaireIndexViewModel.QuestionnaireListItem;

namespace Ubora.Web.Tests._Features.Projects.ApplicableRegulations
{
    public class QuestionnaireIndexViewModelTests
    {
        [Fact]
        public void Factory_Should_Return_All_Non_Stopped_Project_Classifications()
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
            var questionnaires = new PagedListStub<ApplicableRegulationsQuestionnaireAggregate>();
            queryProcessorMock
                .Setup(q => q.Find(
                    new IsFromProjectSpec<ApplicableRegulationsQuestionnaireAggregate> {ProjectId = projectId},
                    null,
                    int.MaxValue, 1))
                .Returns(questionnaires);
            var projectionMock = new Mock<IProjection<ApplicableRegulationsQuestionnaireAggregate, QuestionnaireListItem>>();
            projectionMock.Setup(p => p.Apply(questionnaires))
                .Returns(classifications);
            var viewModelFactory = new QuestionnaireIndexViewModel.Factory(queryProcessorMock.Object, projectionMock.Object);

            // Act
            var model = viewModelFactory.Create(projectId);

            // Assert
            model.Previous.Should().Equal(lastNonStoppedClassification, firstPreviousClassification, secondPreviousClassification);
            model.Last.Should().BeSameAs(lastNonStoppedClassification);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void QuestionnaireListItemProjection_Maps_To_ListItem(bool stopped, bool finished)
        {
            var classification = Mock.Of<ApplicableRegulationsQuestionnaireAggregate>(d => d.IsStopped == stopped)
                .Set(d => d.FinishedAt, finished ? (DateTime?)DateTime.UtcNow : null)
                .Set(d => d.StartedAt, DateTime.UtcNow)
                .Set(d => d.Id, Guid.NewGuid());

            var projection = new QuestionnaireIndexViewModel.QuestionnaireListItemProjection();
            // Act
            var projectedModel = projection.Apply(new[] {classification}.AsQueryable()).Single();
            // Assert
            projectedModel.ShouldBeEquivalentTo(new QuestionnaireListItem
            {
                IsStopped  = stopped,
                StartedAt = classification.StartedAt,
                IsFinished = finished,
                QuestionnaireId = classification.Id
            } );
        }
    }
}
