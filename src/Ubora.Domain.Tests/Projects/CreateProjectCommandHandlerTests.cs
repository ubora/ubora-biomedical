using System;
using System.Collections.Generic;
using System.Text;
using Marten;
using Moq;
using Ubora.Domain.Projects;
using Xunit;

namespace Ubora.Domain.Tests.Projects
{
    public class CreateProjectCommandHandlerTests
    {
        [Fact]
        public void Handle_Creates_New_Project_With_Creator_As_First_Member()
        {
            var command = new CreateProjectCommand();

            var documentSessionMock = new Mock<IDocumentSession>();
            var handler = new CreateProjectCommandHandler(documentSessionMock.Object);

            //documentSessionMock.Verify(x => x.Events.Append());


            // Act
            handler.Handle(command);

            // Assert
        }
    }
}
