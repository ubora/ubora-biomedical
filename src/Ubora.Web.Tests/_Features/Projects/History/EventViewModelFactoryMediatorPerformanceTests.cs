using Autofac;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Encodings.Web;
using Ubora.Domain;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.DeviceClassification;
using Ubora.Domain.Projects.Tasks;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Events;
using Ubora.Domain.Tests;
using Ubora.Domain.Users;
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

        private UserInfo _userInfo;
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

        [Fact(Skip = "Explicit test")]
        public void Returns_TimeElapsed_For_Hundreds_Of_Events_Need_To_Be_Handled()
        {
            var mediator = Container.Resolve<EventViewModelFactoryMediator>();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            CreateProject();

            for (int i = 0; i < 100; i++)
            {
                EditProject();

                AddTask();

                EditProjectDeviceClassification();
            }

            SubmitWorkPackageOneForReview();

            AcceptWorkPackageOneByReview();

            for (int i = 0; i < 100; i++)
            {
                EditWorkPackageTwo();
            }

            var events = Session.Events.FetchStream(_projectId);

            // Act
            var result = events.Select(x => mediator.Create((UboraEvent)x.Data, x.Timestamp)).ToList();
            stopwatch.Stop();

            // Assert
            var timeElapsed = stopwatch.Elapsed;

            throw new InvalidOperationException($"Mediator handled {events.Count} events. Time elapsed: {timeElapsed}");
        }

        private void EditWorkPackageTwo()
        {
            var editedWorkpackageTwoEvent = new WorkpackageTwoStepEdited(
                                initiatedBy: _userInfo,
                                newValue: "new value",
                                title: "new title",
                                stepId: WorkpackageStepIds.DescriptionOfFunctions);
            Session.Events.Append(_projectId, editedWorkpackageTwoEvent);
            Session.SaveChanges();
        }

        private void AcceptWorkPackageOneByReview()
        {
            var workPackageOneReviewAcceptedEvent = new WorkpackageOneReviewAcceptedEvent(
                            initiatedBy: _userInfo,
                            projectId: _projectId,
                            acceptedAt: DateTimeOffset.Now,
                            concludingComment: "comment"
                            );
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
                                initiatedBy: _userInfo,
                                newValue: "new value",
                                title: "new title",
                                stepId: WorkpackageStepIds.DescriptionOfNeeds);
            Session.Events.Append(_projectId, editedWorkpackageOneEvent);
            Session.SaveChanges();
        }

        private void EditProjectDeviceClassification()
        {
            var editedDeviceClassificationEvent = new EditedProjectDeviceClassificationEvent(
                                id: _projectId,
                                newClassification: new Classification("IIb", 3, new List<Guid>()),
                                currentClassification: new Classification("IIa", 3, new List<Guid>()),
                                initiatedBy: _userInfo);
            Session.Events.Append(_projectId, editedDeviceClassificationEvent);
            Session.SaveChanges();
        }

        private void AddTask()
        {
            var taskAddedEvent = new TaskAddedEvent(
                                initiatedBy: _userInfo)
            {
                Id = Guid.NewGuid(),
                ProjectId = _projectId,
                Title = "title",
                Description = $"submitted workpackage 1 for review {StringTokens.WorkpackageOneReview()}"
            };
            Session.Events.Append(_projectId, taskAddedEvent);
            Session.SaveChanges();
        }

        private void CreateProject()
        {
            var projectAddedEvent = new ProjectCreatedEvent(initiatedBy: _userInfo)
            {
                Id = _projectId,
                Title = "Awesome Project"
            };
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

        protected override void RegisterAdditional(ContainerBuilder builder)
        {
            var webAutofacModule = new WebAutofacModule(true);
            builder.RegisterModule(webAutofacModule);
            builder.RegisterInstance(_htmlEncoderMock.Object).SingleInstance();
            builder.RegisterInstance(_urlHelperMock.Object).As<IUrlHelper>().SingleInstance();
        }
    }
}
