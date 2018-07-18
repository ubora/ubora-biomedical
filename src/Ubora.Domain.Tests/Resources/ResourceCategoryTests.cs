using System;
using FluentAssertions;
using Ubora.Domain.Resources;
using Ubora.Domain.Resources.Events;
using Xunit;

namespace Ubora.Domain.Tests.Resources
{
    public class ResourceCategoryTests
    {
        [Fact]
        public void Resource_Category_Can_Not_Be_Its_Own_Parent()
        {
            var category = new ResourceCategory().Set(c => c.Id, Guid.NewGuid());

            // Act
            Action act = () => category.Apply(new ResourceCategoryEditedEvent(new DummyUserInfo(), category.Id, "", "", category.Id, 0));

            // Assert
            act.ShouldThrow<InvalidOperationException>();
        }
    }
}
