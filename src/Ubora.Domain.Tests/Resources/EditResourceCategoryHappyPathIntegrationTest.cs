using System;
using System.Linq;
using FluentAssertions;
using Ubora.Domain.Resources;
using Ubora.Domain.Resources.Commands;
using Xunit;

namespace Ubora.Domain.Tests.Resources
{
    public class EditResourceCategoryHappyPathIntegrationTest : IntegrationFixture
    {
        [Fact]
        public void Resource_Category_Can_Be_Edited()
        {
            var categoryId = Guid.NewGuid();
            new ResourceCategorySeeder().WithId(categoryId).WithParentCategory(Guid.NewGuid()).Seed(this);

            Guid? parentCategoryId = null;
            var title = Guid.NewGuid().ToString();
            var description = Guid.NewGuid().ToString();
            var menuPriority = 123;

            // Act
            var result = Processor.Execute(new EditResourceCategoryCommand
            {
                CategoryId = categoryId,
                ParentCategoryId = parentCategoryId,
                Title = title,
                Description = description,
                MenuPriority = menuPriority,
                Actor = new DummyUserInfo()
            });

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