using Autofac;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Encodings.Web;
using Marten.Events;
using Ubora.Domain;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects.Assignments.Events;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Events;
using Ubora.Domain.Projects._Events;
using Ubora.Domain.Tests;
using Ubora.Domain.Users.Commands;
using Ubora.Web._Features.Projects.History._Base;
using Ubora.Web.Data;
using Ubora.Web.Infrastructure;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.History
{
    public class EventViewModelFactoryMediatorPerformanceTests : IntegrationFixture
    {
        private readonly Mock<HtmlEncoder> _htmlEncoderMock;
        private readonly Mock<IUrlHelper> _urlHelperMock;

        private readonly UserInfo _userInfo;
        private readonly Guid _projectId;

        public EventViewModelFactoryMediatorPerformanceTests()
        {
            _htmlEncoderMock = new Mock<HtmlEncoder>();

            _htmlEncoderMock.Setup(x => x.Encode(It.IsAny<string>()))
                .Returns<string>(input => input);

            _urlHelperMock = new Mock<IUrlHelper>();

            _userInfo = CreateUserIfo();
            _projectId = Guid.NewGuid();

        }

        protected override void RegisterAdditional(ContainerBuilder builder)
        {
            var webAutofacModule = new WebAutofacModule(true);
            builder.RegisterModule(webAutofacModule);
            builder.RegisterInstance(_htmlEncoderMock.Object).SingleInstance();
            builder.RegisterInstance(_urlHelperMock.Object).As<IUrlHelper>().SingleInstance();
        }

        [Fact(Skip = "Explicit test")]
        public void Returns_TimeElapsed_For_Hundreds_Of_Events_Need_To_Be_Handled()
        {
            CreateProject();

            for (int i = 0; i < 100; i++)
            {
                EditProject();

                AddTask();
            }

            SubmitWorkPackageOneForReview();

            AcceptWorkPackageOneByReview();

            for (int i = 0; i < 100; i++)
            {
                EditWorkPackageTwo();
            }

            var events = Session.Events.FetchStream(_projectId);
            var stopwatch = new Stopwatch();

            var messageBuilder = new StringBuilder();

            // Act
            stopwatch.Start();
            RunMediator(events);
            stopwatch.Stop();

            // Assert
            messageBuilder.AppendLine($"Mediator handled {events.Count} events. It took {stopwatch.Elapsed.TotalSeconds} seconds");

            throw new Exception(messageBuilder.ToString());
        }

        private void RunMediator(IReadOnlyList<IEvent> events)
        {
            var mediator = Container.Resolve<EventViewModelFactoryMediator>();

            foreach (var e in events)
            {
                mediator.Create((UboraEvent)e.Data, e.Timestamp);
            }
        }

        private void EditWorkPackageTwo()
        {
            var editedWorkpackageTwoEvent = new WorkpackageTwoStepEdited(
                projectId: _projectId,
                initiatedBy: _userInfo,
                newValue: "new value",
                title: "new title",
                stepId: "ClinicalNeeds");
            Session.Events.Append(_projectId, editedWorkpackageTwoEvent);
            Session.SaveChanges();
        }

        private void AcceptWorkPackageOneByReview()
        {
            var workPackageOneReviewAcceptedEvent = new WorkpackageOneReviewAcceptedEvent(
                initiatedBy: _userInfo,
                projectId: _projectId,
                acceptedAt: DateTimeOffset.Now,
                concludingComment: "comment");
            Session.Events.Append(_projectId, workPackageOneReviewAcceptedEvent);
            Session.SaveChanges();
        }

        private void SubmitWorkPackageOneForReview()
        {
            var workPackageOneReviewSubmittedEvent = new WorkpackageOneSubmittedForReviewEvent(
                initiatedBy: _userInfo,
                projectId: _projectId,
                reviewId: Guid.NewGuid(),
                submittedAt: DateTimeOffset.Now);
            Session.Events.Append(_projectId, workPackageOneReviewSubmittedEvent);
            Session.SaveChanges();
        }

        private void EditProject()
        {
            var editedWorkpackageOneEvent = new WorkpackageOneStepEditedEvent(
                projectId: _projectId,
                initiatedBy: _userInfo,
                newValue: "new value",
                title: "new title",
                stepId: "ClinicalNeeds");
            Session.Events.Append(_projectId, editedWorkpackageOneEvent);
            Session.SaveChanges();
        }

        private void AddTask()
        {
            var taskAddedEvent = new AssignmentAddedEvent(
                initiatedBy: _userInfo,
                id: Guid.NewGuid(),
                projectId: _projectId,
                title: "title",
                description: $"submitted workpackage 1 for review {StringTokens.WorkpackageOneReview(_projectId)}",
                assigneeIds: null
            );
            Session.Events.Append(_projectId, taskAddedEvent);
            Session.SaveChanges();
        }

        private void CreateProject()
        {
            var projectAddedEvent = new ProjectCreatedEvent(
                initiatedBy: _userInfo,
                projectId: _projectId,
                title: "Awesome Project",
                clinicalNeed: "",
                areaOfUsage: "",
                potentialTechnology: "",
                gmdn: "");

            Session.Events.Append(_projectId, projectAddedEvent);
            Session.SaveChanges();
        }

        private UserInfo CreateUserIfo()
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = "email@agileworks.eu",
                Email = "email@agileworks.eu"
            };
            var commandResult = Processor.Execute(new CreateUserProfileCommand
            {
                UserId = user.Id,
                Email = user.Email,
                FirstName = "Test",
                LastName = "User"
            });

            var userInfo = new UserInfo(user.Id, "Test User");
            return userInfo;
        }

    }
}
