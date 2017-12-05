using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Baseline;
using FluentAssertions;
using Marten.Events;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects.History;
using Ubora.Domain.Projects._Events;
using Ubora.Domain.Projects._Specifications;
using Ubora.Web._Features.Projects.History;
using Ubora.Web._Features.Projects.History._Base;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.History
{
    public class HistoryControllerTests : ProjectControllerTestsBase
    {
        [Fact]
        public void History_Must_Return_Project_EventLogEntries()
        {
            var projectId = ProjectId;
            var log1 = EventLogEntry.FromEvent(new Event<ProjectEventStub>(new ProjectEventStub()));
            Thread.Sleep(3); // to guarantee that log2 has Timestamp with a later time
            var log2 = EventLogEntry.FromEvent(new Event<ProjectEventStub>(new ProjectEventStub()));

            QueryProcessorMock.Setup(
                q => q.Find<EventLogEntry>(new IsFromProjectSpec<EventLogEntry> {ProjectId = projectId}))
                .Returns(new PagedListStub<EventLogEntry> { log1, log2});

            var eventViewModelFactoryMediatorMock = new Mock<IEventViewModelFactoryMediator>();
            var log1ViewModel = Mock.Of<IEventViewModel>();
            var log2ViewModel = Mock.Of<IEventViewModel>();
            eventViewModelFactoryMediatorMock.Setup(f => f.Create(log1.Event, log1.Timestamp)).Returns(log1ViewModel);
            eventViewModelFactoryMediatorMock.Setup(f => f.Create(log2.Event, log2.Timestamp)).Returns(log2ViewModel);


            var controller = new HistoryController(eventViewModelFactoryMediatorMock.Object);
            SetUpForTest(controller);

            // Act
            var result = controller.History() as ViewResult;
            
            // Assert
            var viewModel = result.Model as IEventViewModel[];
            viewModel.Length.Should().Be(2);
            viewModel[0].Should().Be(log2ViewModel);
            viewModel[1].Should().Be(log1ViewModel);
        }
    }

    public class ProjectEventStub : ProjectEvent
    {
        public ProjectEventStub() : base(new UserInfo(Guid.NewGuid(), Guid.NewGuid().ToString()), Guid.NewGuid())
        {
        }

        public override string GetDescription()
        {
            return ProjectId.ToString();
        }
    }
}
