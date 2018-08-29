using System;
using FluentAssertions;
using Ubora.Domain.Resources;
using Ubora.Domain.Resources.Events;
using Xunit;

namespace Ubora.Domain.Tests.Resources
{
    public class ResourcePageTests : IntegrationFixture
    {
        [Fact]
        public void Does_Not_Allow_Edits_When_When_Content_Version_Has_Changed()
        {
            var resourcePage = new ResourcePage()
                .Set(page => page.BodyVersion, 2);

            // Act
            Action act = () => resourcePage.Apply(new ResourcePageBodyEditedEvent(
                initiatedBy: new DummyUserInfo(),
                resourcePageId: resourcePage.Id,
                body: new QuillDelta("{asdf}"),
                previousBodyVersion: 1));

            // Assert
            act.ShouldThrow<InvalidOperationException>()
                .Where(ex => ex.Message.Contains("version"));
        }
    }
}