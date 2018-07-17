using System;
using System.IO;
using System.Linq;
using Autofac;
using FluentAssertions;
using Moq;
using Ubora.Domain;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Resources;
using Ubora.Domain.Resources.Commands;
using Ubora.Domain.Resources.Events;
using Ubora.Domain.Tests;
using Ubora.Web.Infrastructure;
using Ubora.Web.Infrastructure.Storage;
using Xunit;

namespace Ubora.Web.Tests._Areas.ResourcesArea.ResourcePages.CommandHandlers
{
    public class UploadResourceFileCommandHandlerIntegrationTests : IntegrationFixture
    {
        private Mock<IUboraStorageProvider> _uboraStorageProviderMock;

        protected override void RegisterAdditional(ContainerBuilder builder)
        {
            builder.RegisterModule(new WebAutofacModule(useSpecifiedPickupDirectory: true));

            _uboraStorageProviderMock = new Mock<IUboraStorageProvider>();
            builder.RegisterInstance(_uboraStorageProviderMock.Object);
        }

        [Fact]
        public void Resource_File_Can_Be_Uploaded()
        {
            var resourcePageId = Guid.Parse("C962707F-BBAD-4E18-9225-ACEE4205D73F");
            this.Processor.Execute(new CreateResourcePageCommand
            {
                ResourcePageId = resourcePageId,
                Title = "testTitle",
                Body = new QuillDelta("testValue"),
                Actor = new DummyUserInfo(),
                MenuPriority = 123
            });

            using (var fileStream = new MemoryStream())
            {
                var command = new UploadResourceFileCommand
                {
                    FileId = Guid.Parse("A9896CBA-EE90-435D-AF6F-FEE5F6A832D1"),
                    ResourcePageId = resourcePageId,
                    Actor = new DummyUserInfo(),
                    FileName = "testName.jpg",
                    FileSize = 123,
                    FileStream = fileStream
                };

                // Act
                var result = this.Processor.Execute(command);

                // Assert
                result.IsSuccess.Should().BeTrue();

                var entityProjection = this.Session.Load<ResourceFile>(command.FileId);

                entityProjection.Id.Should().Be(command.FileId);
                entityProjection.FileName.Should().Be("testName.jpg");
                entityProjection.FileSize.Should().Be(123);

                var expectedBlobLocation = new BlobLocation("resourcepage-C962707F-BBAD-4E18-9225-ACEE4205D73F", "repository/A9896CBA-EE90-435D-AF6F-FEE5F6A832D1/testName.jpg");
                entityProjection.BlobLocation.Should().Be(expectedBlobLocation);

                _uboraStorageProviderMock
                    .Verify(x => x.SavePublic(expectedBlobLocation, command.FileStream),
                    Times.Once);

                var @event = (ResourceFileUploadedEvent)this.Session.Events
                    .FetchStream(resourcePageId).Select(martenEvent => martenEvent.Data).OfType<ResourceFileUploadedEvent>().Single();
                @event.InitiatedBy.ShouldBeEquivalentTo(command.Actor);
            }
        }
    }
}