using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Moq;
using Ubora.Domain.Infrastructure;
using Ubora.Web._Features.Projects.Workpackages.Steps;

namespace Ubora.Web.Tests._Features.Projects.Workpackages
{
    public class WorkpackageOneReviewControllerTests : ProjectControllerTestsBase
    {
        private readonly WorkpackageOneController _workpackageOneReviewController;

        private readonly Mock<ICommandQueryProcessor> _processorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IAuthorizationService> _authorizationServiceMock;

        public WorkpackageOneReviewControllerTests()
        {
            _processorMock = new Mock<ICommandQueryProcessor>();
            _mapperMock = new Mock<IMapper>();
            _authorizationServiceMock = new Mock<IAuthorizationService>();

            _workpackageOneReviewController = new WorkpackageOneController(_processorMock.Object, _mapperMock.Object, _authorizationServiceMock.Object);
        }
    }
}
