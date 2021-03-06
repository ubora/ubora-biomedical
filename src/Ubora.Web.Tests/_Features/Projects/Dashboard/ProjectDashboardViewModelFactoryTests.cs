﻿using System;
using AutoMapper;
using FluentAssertions;
using Moq;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects._Specifications;
using Ubora.Domain.Questionnaires.DeviceClassifications;
using Ubora.Domain.Questionnaires.DeviceClassifications.DeviceClasses;
using Ubora.Domain.Questionnaires.DeviceClassifications.Queries;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Web.Tests.Fakes;
using Ubora.Web._Features.Projects.Dashboard;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.Dashboard
{
    public class ProjectDashboardViewModelFactoryTests
    {
        private readonly Mock<IMapper> _autoMapperMock;
        private readonly ProjectDashboardViewModel.Factory _factoryUnderTest;
        private readonly Mock<ImageStorageProvider> _storageProviderMock;
        private readonly Mock<IQueryProcessor> _queryProcessorMock;

        public ProjectDashboardViewModelFactoryTests()
        {
            _autoMapperMock = new Mock<IMapper>();
            _storageProviderMock = new Mock<ImageStorageProvider>();
            _queryProcessorMock = new Mock<IQueryProcessor>();
            _factoryUnderTest = new ProjectDashboardViewModel.Factory(_autoMapperMock.Object, _storageProviderMock.Object, _queryProcessorMock.Object);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Create_Creates_ViewModel(
            bool isProjectMember)
        {
            var projectMock = new Mock<Project>();
            var project = projectMock.Object
                .Set(x => x.Id, Guid.NewGuid());
            var userId = Guid.NewGuid();

            var expectedModel = new ProjectDashboardViewModel();

            _autoMapperMock.Setup(x => x.Map<ProjectDashboardViewModel>(projectMock.Object))
                .Returns(expectedModel);

            projectMock.Setup(x => x.DoesSatisfy(new HasMember(userId)))
                .Returns(isProjectMember);

            var questinnaireTreeMock = new Mock<DeviceClassificationQuestionnaireTree>();

            var expectedDeviceClass = DeviceClass.TwoA;

            questinnaireTreeMock.Setup(x => x.GetHighestRiskDeviceClass())
                .Returns(expectedDeviceClass);

            var latestDeviceClassification = new DeviceClassificationAggregate()
                .Set(x => x.QuestionnaireTree, questinnaireTreeMock.Object);

            _queryProcessorMock
                .Setup(x => x.ExecuteQuery(It.Is<LatestFinishedProjectDeviceClassificationQuery>(q => q.ProjectId == project.Id)))
                .Returns(latestDeviceClassification);

            var user = FakeClaimsPrincipalFactory.CreateAuthenticatedUser(userId: userId);

            // Act
            var result = _factoryUnderTest.Create(project, user);

            // Assert
            result.Should().BeSameAs(expectedModel);
            result.IsProjectMember.Should().Be(isProjectMember);
            result.DeviceClassification.Should().Be(expectedDeviceClass.Name);
        }

        [Fact]
        public void Create_Sets_IsProjectMember_To_False_When_Unauthenticated()
        {
            var projectMock = new Mock<Project>();
            var project = projectMock.Object;

            _autoMapperMock.Setup(x => x.Map<ProjectDashboardViewModel>(projectMock.Object))
                .Returns(new ProjectDashboardViewModel());

            // Act
            var result = _factoryUnderTest.Create(project, user: FakeClaimsPrincipalFactory.CreateAnonymousUser());

            // Assert
            result.IsProjectMember.Should().BeFalse();

            projectMock.Verify(x => x.DoesSatisfy(It.IsAny<HasMember>()), Times.Never);
        }
    }
}
