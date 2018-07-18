using FluentAssertions;
using Ubora.Domain.Resources;
using Xunit;

namespace Ubora.Domain.Tests.Resources
{
    public class ResourcesMenuIntegrationTests : IntegrationFixture
    {
        [Fact]
        public void HighestPriorityResourcePageLink_Is_Highest_Priority_ResourcePage_From_Highest_Priority_Category()
        {
            // Assert (1/2)
            var highestPriorityCategoryWithoutPages = new ResourceCategorySeeder().WithMenuPriority(999).Seed(this);
            var category1 = new ResourceCategorySeeder().WithMenuPriority(0).Seed(this);
            var category2 = new ResourceCategorySeeder().WithMenuPriority(10).Seed(this);

            var expectedPage1 = new ResourcePageSeeder().WithMenuPriority(10).WithParentCategory(category2.Id).Seed(this);
            new ResourcePageSeeder().WithMenuPriority(-10).WithParentCategory(null).Seed(this);
            new ResourcePageSeeder().WithMenuPriority(20).WithParentCategory(category1.Id).Seed(this);
            new ResourcePageSeeder().WithMenuPriority(0).WithParentCategory(category2.Id).Seed(this);

            // Act (1/2)
            Session.Load<ResourcesMenu>(ResourcesMenu.SingletonId)
                .HighestPriorityResourcePageLink
                .ShouldSatisfy(x => x.Id == expectedPage1.Id);

            // Assert (2/2)
            var expectedPage2 = new ResourcePageSeeder().WithMenuPriority(999).WithParentCategory(null).Seed(this);

            // Act (2/2)
            Session.Load<ResourcesMenu>(ResourcesMenu.SingletonId)
                .HighestPriorityResourcePageLink
                .ShouldSatisfy(x => x.Id == expectedPage2.Id);
        }
    }
}
