using System;
using System.Linq;
using FluentAssertions;
using Ubora.Domain.Resources;
using Xunit;

namespace Ubora.Domain.Tests.Resources
{
    public class CreateResourceCategoryHappyPathIntegrationTest : IntegrationFixture
    {
        [Fact]
        public void Resource_Category_Can_Be_Created()
        {
            var categoryId = Guid.NewGuid();
            var parentCategoryId = Guid.NewGuid();
            var title = Guid.NewGuid().ToString();
            var description = Guid.NewGuid().ToString();
            var menuPriority = 123;

            // Act
            new ResourceCategorySeeder()
                .WithId(categoryId)
                .WithParentCategory(parentCategoryId)
                .WithTitle(title)
                .WithDescription(description)
                .WithMenuPriority(menuPriority)
                .Seed(this);

            // Assert
            var createdCategory = Session.Load<ResourceCategory>(categoryId);
            createdCategory.ParentCategoryId.Should().Be(parentCategoryId);
            createdCategory.Title.Should().Be(title);
            createdCategory.Description.Should().Be(description);
            createdCategory.MenuPriority.Should().Be(menuPriority);

            // Link should be in menu
            Session.Load<ResourcesMenu>(ResourcesMenu.SingletonId)
                .Links.OfType<ResourceCategoryLink>()
                .Any(link =>
                    link.Id == categoryId
                    && link.Title == title
                    && link.MenuPriority == menuPriority)
                .Should().BeTrue();
        }
    }
}
