using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Marten;
using Moq;
using Ubora.Domain.ApplicableRegulations;
using Ubora.Domain.ApplicableRegulations.Events;
using Xunit;

namespace Ubora.Domain.Tests.ApplicableRegulations
{
    public class ApplicableRegulationsQuestionnaireAggregateTests
    {

        [Fact]
        public void Apply_With_Stopped_Event_Sets_Value_To_FinishedAt()
        {
            var stoppedEvent = new ApplicableRegulationsQuestionnaireStoppedEvent(new DummyUserInfo(), new Guid());
            var aggregate = new ApplicableRegulationsQuestionnaireAggregate();

            // Act
            aggregate.Apply(stoppedEvent);

            // Assert
            aggregate.FinishedAt.Should().HaveValue();
            aggregate.FinishedAt.Should().BeCloseTo(DateTime.UtcNow);
            aggregate.IsFinished.Should().BeTrue();
        }
        [Fact]
        public void Apply_With_Stopped_Event_Throws_When_Questionnaire_Already_Finished()
        {
            var stoppedEvent = new ApplicableRegulationsQuestionnaireStoppedEvent(new DummyUserInfo(), new Guid());

            var finishTime = new DateTime(2015);

            var aggregate = new ApplicableRegulationsQuestionnaireAggregate();
            aggregate.Set(x => x.FinishedAt, finishTime);

            // Act
            Action result = () =>
            {
                aggregate.Apply(stoppedEvent);
            };

            // Assert
            result.ShouldThrow<InvalidOperationException>();
        }
        [Fact]
        public void Apply_With_Stopped_Event_Sets_IsStopped_True()
        {
            var stoppedEvent = new ApplicableRegulationsQuestionnaireStoppedEvent(new DummyUserInfo(), new Guid());

            var aggregate = new ApplicableRegulationsQuestionnaireAggregate();

            var mock = new Mock<Questionnaire>();

            mock.Setup(x => x.FindNextUnansweredQuestion())
                .Returns(Mock.Of<Question>());
            aggregate.Set(x => x.Questionnaire, mock.Object);

            // Act
            aggregate.Apply(stoppedEvent);

            // Assert
            aggregate.IsStopped.Should().BeTrue();
        }
        [Fact]
        public void Applicable_Regulatations_Aggregate_IsStopped_Is_False_At_Start()
        {
            var aggregate = new ApplicableRegulationsQuestionnaireAggregate();

            // Act
            var result = aggregate.IsStopped;

            // Assert
            result.Should().BeFalse();
        }
        [Fact]
        public void When_Aggregate_Has_FinishTime_And_Unanswered_Question_Left_IsStopped_Should_Be_True()
        {
            var finishTime = new DateTime(2015);
            var aggregate = new ApplicableRegulationsQuestionnaireAggregate();

            var mock = new Mock<Questionnaire>();

            mock.Setup(x => x.FindNextUnansweredQuestion())
                .Returns(Mock.Of<Question>());

            aggregate.Set(x => x.FinishedAt, finishTime)
                .Set(x => x.Questionnaire, mock.Object);

            // Act
            var result = aggregate.IsStopped;

            // Assert
            result.Should().BeTrue();
        }
        [Fact]
        public void When_Aggregate_Has_FinishTime_But_No_Unanswered_Questions_Left_IsStopped_Should_Be_False()
        {
            var finishTime = new DateTime(2015);
            var aggregate = new ApplicableRegulationsQuestionnaireAggregate();

            var mock = new Mock<Questionnaire>();

            mock.Setup(x => x.FindNextUnansweredQuestion())
                .Returns((Question)null);

            aggregate.Set(x => x.FinishedAt, finishTime)
                .Set(x => x.Questionnaire, mock.Object);

            // Act
            var result = aggregate.IsStopped;

            // Assert
            result.Should().BeFalse();
        }
    }
}
