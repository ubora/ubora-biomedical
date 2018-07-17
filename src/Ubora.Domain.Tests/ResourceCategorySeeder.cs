using System;
using Ubora.Domain.Resources;
using Ubora.Domain.Resources.Commands;

namespace Ubora.Domain.Tests
{
    public class ResourceCategorySeeder
    {
        public Guid CategoryId { get; private set; } = Guid.NewGuid();
        public Guid? ParentCategoryId { get; private set; }
        public string Title { get; private set; } = Guid.NewGuid().ToString();
        public string Description { get; private set; }
        public int MenuPriority { get; private set; }

        public ResourceCategorySeeder WithId(Guid categoryId)
        {
            CategoryId = categoryId;
            return this;
        }

        public ResourceCategorySeeder WithTitle(string title)
        {
            Title = title;
            return this;
        }

        public ResourceCategorySeeder WithDescription(string description)
        {
            Description = description;
            return this;
        }

        public ResourceCategorySeeder WithMenuPriority(int menuPriority)
        {
            MenuPriority = menuPriority;
            return this;
        }

        public ResourceCategorySeeder WithParentCategory(Guid? categoryId)
        {
            ParentCategoryId = categoryId;
            return this;
        }

        public ResourceCategory Seed(IntegrationFixture fixture)
        {
            if (ParentCategoryId.HasValue)
            {
                CreateCategoryIfNecessary(ParentCategoryId.Value, fixture);
            }

            fixture.Processor.Execute(new CreateResourceCategoryCommand
            {
                Title = Title,
                CategoryId = CategoryId,
                Description = Description,
                MenuPriority = MenuPriority,
                ParentCategoryId = ParentCategoryId,
                Actor = new DummyUserInfo()
            });

            return fixture.Processor.FindById<ResourceCategory>(CategoryId);
        }

        private void CreateCategoryIfNecessary(Guid categoryId, IntegrationFixture fixture)
        {
            var category = fixture.Processor.FindById<ResourceCategory>(categoryId);
            if (category == null)
            {
                new ResourceCategorySeeder().WithId(categoryId).Seed(fixture);
            }
        }
    }
}