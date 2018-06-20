using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Ubora.Web._Features.ResourceRepository;
using Xunit;

namespace Ubora.Web.Tests._Features.ResourceRepository
{
    public class ResourceRepositoryControllerTests : UboraControllerTestsBase
    {
        private Mock<ResourceRepositoryController> _controllerMock;

        public ResourceRepositoryControllerTests()
        {
            _controllerMock = new Mock<ResourceRepositoryController>();
        }

        public ResourceRepositoryController ControllerUnderTest => _controllerMock.Object;

        [Fact]
        public void Foo()
        {

        }
    }
}
