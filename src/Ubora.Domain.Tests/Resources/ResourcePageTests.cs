using System;
using FluentAssertions;
using Ubora.Domain.Resources;
using Xunit;

namespace Ubora.Domain.Tests.Resources
{
    public class ResourcePageTests : IntegrationFixture
    {
        [Fact]
        public void Does_Not_Allow_Edits_When_When_Content_Version_Has_Changed()
        {
            var resourcePage = new ResourcePage()
                .Set(page => page.ContentVersion, Guid.NewGuid());

            // Act
            Action act = () => resourcePage.Apply(new ResourceContentEditedEvent(
                initiatedBy: new DummyUserInfo(),
                content: new ResourceContent("abc", "dfg"), 
                previousContentVersion: Guid.NewGuid()));

            // Assert
            act.ShouldThrow<InvalidOperationException>()
                .Where(ex => ex.Message.Contains("version"));
        }
    }
}