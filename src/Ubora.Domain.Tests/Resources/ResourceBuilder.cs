using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Resources;
using Ubora.Domain.Users;

namespace Ubora.Domain.Tests.Resources
{
    /// <summary>
    /// Test-data builder
    /// </summary>
    public class ResourceBuilder
    {
        private Guid ResourceId { get; set; } = Guid.NewGuid();
        private ResourceContent Content { get; set; } = new ResourceContent(title: Guid.NewGuid().ToString(), body: Guid.NewGuid().ToString());
        private Guid CreatorUserId { get; set; } = Guid.NewGuid();

        public ResourceBuilder WithId(Guid resourceId)
        {
            ResourceId = resourceId;
            return this;
        }
        
        public ResourceBuilder WithContent(ResourceContent content)
        {
            Content = content;
            return this;
        }
        
        public ResourceBuilder WithCreator(Guid userId)
        {
            CreatorUserId = userId;
            return this;
        }
        
        public Resource Build(IntegrationFixture fixture)
        {
            CreateUserProfileIfNecessary(CreatorUserId, fixture);
            
            fixture.Processor.Execute(new CreateResourcePageCommand
            {
                ResourceId = ResourceId,
                Content = Content,
                Actor = new UserInfo(CreatorUserId, "testName")
            });
            
            return fixture.Processor.FindById<Resource>(ResourceId);
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