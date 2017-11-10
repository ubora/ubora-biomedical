using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Moq;
using TwentyTwenty.Storage;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects._Specifications;
using Ubora.Domain.Questionnaires.DeviceClassifications;
using Ubora.Domain.Questionnaires.DeviceClassifications.DeviceClasses;
using Ubora.Domain.Questionnaires.DeviceClassifications.Queries;
using Ubora.Web.Infrastructure.ImageServices;
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
        public void Foo(
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

            var expectedDeviceClass = new DeviceClassTwoA(new ChosenAnswerDeviceClassCondition[0]);

            questinnaireTreeMock.Setup(x => x.GetHeaviestDeviceClass())
                .Returns(expectedDeviceClass);

            var latestDeviceClassification = new DeviceClassificationAggregate()
                .Set(x => x.QuestionnaireTree, questinnaireTreeMock.Object);

            _queryProcessorMock
                .Setup(x => x.ExecuteQuery(It.Is<LatestFinishedProjectDeviceClassificationQuery>(q => q.ProjectId == project.Id)))
                .Returns(latestDeviceClassification);

            // Act
            var result = _factoryUnderTest.Create(project, userId);

            // Assert
            result.Should().BeSameAs(expectedModel);
            result.IsProjectMember.Should().Be(isProjectMember);
            result.DeviceClassification.Should().Be(expectedDeviceClass.Name);
        }
    }
}
