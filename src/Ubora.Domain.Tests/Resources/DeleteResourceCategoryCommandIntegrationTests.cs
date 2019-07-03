using System;
using System.Linq;
using FluentAssertions;
using Ubora.Domain.Resources;
using Ubora.Domain.Resources.Commands;
using Xunit;

namespace Ubora.Domain.Tests.Resources
{
    public class DeleteResourceCategoryCommandIntegrationTests : IntegrationFixture
    {
        [Fact]
        public void Resource_Category_Can_Be_Deleted()
        {
            var categoryId = Guid.NewGuid();
            new ResourceCategorySeeder().WithId(categoryId).Seed(this);

            // Act
            var result = Processor.Execute(new DeleteResourceCategoryCommand
            {
                ResourceCategoryId = categoryId,
                Actor = new DummyUserInfo()
            });

            // Assert
            result.IsSuccess.Should().BeTrue();

            var deletedCategory = Session.Load<ResourceCategory>(categoryId);

            Session.Tenant.MetadataFor(deletedCategory)
                .Deleted.Should().BeTrue();

            // Link should be removed from menu
            Session.Load<ResourcesMenu>(ResourcesMenu.SingletonId)
                .Links.Any(link => link.Id == categoryId)
                .Should().BeFalse();
        }

        [Fact]
        public void Resource_Category_Can_Not_Be_Deleted_When_It_Has_Children()
        {
            var categoryId = Guid.NewGuid();
            new ResourceCategorySeeder().WithId(categoryId).Seed(this);
            new ResourceCategorySeeder().WithParentCategory(categoryId).Seed(this);

            // Act
            Action act = () => Processor.Execute(new DeleteResourceCategoryCommand
            {
                ResourceCategoryId = categoryId,
                Actor = new DummyUserInfo()
            });

            // Assert
            act.ShouldThrow<InvalidOperationException>();
        }
    }
}
