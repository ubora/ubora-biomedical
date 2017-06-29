using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
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

        public WorkpackageOneReviewControllerTests()
        {
            _processorMock = new Mock<ICommandQueryProcessor>();
            _mapperMock = new Mock<IMapper>();

            _workpackageOneReviewController = new WorkpackageOneController(_processorMock.Object, _mapperMock.Object);
        }


    }
}
