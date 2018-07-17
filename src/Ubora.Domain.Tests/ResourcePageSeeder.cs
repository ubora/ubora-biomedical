using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Resources;
using Ubora.Domain.Resources.Commands;
using Ubora.Domain.Users;

namespace Ubora.Domain.Tests
{
    /// <summary>
    /// Test-data builder
    /// </summary>
    public class ResourcePageSeeder
    {
        public Guid ResourcePageId { get; private set; } = Guid.NewGuid();
        public Guid ResourcePageCreatorUserId { get; private set; } = Guid.NewGuid();
        public string Title { get; private set; } = Guid.NewGuid().ToString();
        public int MenuPriority { get; private set; }
        public Guid? ParentCategoryId { get; private set; }
        public QuillDelta Body { get; private set; } = new QuillDelta(Guid.NewGuid().ToString());

        public ResourcePageSeeder WithId(Guid resourcePageId)
        {
            ResourcePageId = resourcePageId;
            return this;
        }

        public ResourcePageSeeder WithCreator(Guid userId)
        {
            ResourcePageCreatorUserId = userId;
            return this;
        }

        public ResourcePageSeeder WithTitle(string title)
        {
            Title = title;
            return this;
        }

        public ResourcePageSeeder WithBody(QuillDelta body)
        {
            Body = body;
            return this;
        }

        public ResourcePageSeeder WithMenuPriority(int menuPriority)
        {
            MenuPriority = menuPriority;
            return this;
        }

        public ResourcePageSeeder WithParentCategory(Guid? categoryId)
        {
            ParentCategoryId = categoryId;
            return this;
        }

        public ResourcePage Seed(IntegrationFixture fixture)
        {
            CreateUserProfileIfNecessary(ResourcePageCreatorUserId, fixture);

            if (ParentCategoryId.HasValue)
            {
                CreateCategoryIfNecessary(ParentCategoryId.Value, fixture);
            }

            fixture.Processor.Execute(new CreateResourcePageCommand
            {
                ResourcePageId = ResourcePageId,
                Title = Title,
                Body = Body,
                MenuPriority = MenuPriority,
                ParentCategoryId = ParentCategoryId,
                Actor = new UserInfo(ResourcePageCreatorUserId, "")
            });

            return fixture.Processor.FindById<ResourcePage>(ResourcePageId);
        }

        private void CreateCategoryIfNecessary(Guid categoryId, IntegrationFixture fixture)
        {
            var category = fixture.Processor.FindById<ResourceCategory>(categoryId);
            if (category == null)
            {
                new ResourceCategorySeeder().WithId(categoryId).Seed(fixture);
            }
        }

        private void CreateUserProfileIfNecessary(Guid userId, IntegrationFixture fixture)
        {
            var userProfile = fixture.Processor.FindById<UserProfile>(userId);
            if (userProfile == null)
            {
                fixture.Create_User(userId);
            }
        }
    }
}