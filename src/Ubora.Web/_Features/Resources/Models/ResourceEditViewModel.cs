﻿using Ubora.Domain.Resources;

namespace Ubora.Web._Features.Resources.Models
{
    public class ResourceEditViewModel : ResourceEditPostModel
    {
        public string Title { get; set; }

        public class Factory
        {
            public ResourceEditViewModel Create(ResourcePage resourcePage)
            {
                return new ResourceEditViewModel
                {
                    ResourceId = resourcePage.Id,
                    Body = resourcePage.Content.Body.Value,
                    Title = resourcePage.Content.Title,
                    ContentVersion = resourcePage.ContentVersion
                };
            }
        }
    }
}