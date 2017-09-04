﻿using System;
using FluentAssertions;
using Marten;
using Moq;
using Ubora.Domain.ApplicableRegulations;
using Ubora.Domain.ApplicableRegulations.Queries;
using Ubora.Domain.ApplicableRegulations.Specifications;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Xunit;

namespace Ubora.Domain.Tests.ApplicableRegulations
{
    public class StartQuestionnaireCommandHandlerTests
    {
        private readonly StartQuestionnaireCommand.Handler _handlerUnderTest;
        private readonly Mock<IDocumentSession> _documentSessionMock;
        private readonly Mock<IQueryProcessor> _queryProcessorMock;

        public StartQuestionnaireCommandHandlerTests()
        {
            _documentSessionMock = new Mock<IDocumentSession>(MockBehavior.Strict);
            _queryProcessorMock = new Mock<IQueryProcessor>();
            _handlerUnderTest = new StartQuestionnaireCommand.Handler(_documentSessionMock.Object, _queryProcessorMock.Object);
        }

        [Fact]
        public void Returns_Failed_Result_When_Project_Has_Unfinished_Questionnaire()
        {
            var project = new Project();
            var command = new StartQuestionnaireCommand
            {
                ProjectId = project.Id,
                NewQuestionnaireId = Guid.NewGuid(),
                Actor = new DummyUserInfo()
            };

            _documentSessionMock.Setup(x => x.Load<Project>(command.ProjectId))
                .Returns(project);

            _queryProcessorMock
                .Setup(x => x.ExecuteQuery(It.Is<ActiveApplicableRegulationsQuestionnaireQuery>(q => q.ProjectId == project.Id)))
                .Returns(new ProjectQuestionnaireAggregate());

            // Act
            var result = _handlerUnderTest.Handle(command);

            // Assert
            result.IsSuccess.Should().BeFalse();
        }
    }
}
