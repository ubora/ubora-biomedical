﻿using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Resources;
using Ubora.Domain.Resources.Events;

namespace Ubora.Web._Areas.ResourcesArea.ResourcePages.Models
{
    public class ResourceIndexViewModel
    {
        public Guid ResourceId { get; set; }
        public string Title { get; set; }

        public class Mapper : Projection<ResourcePage, ResourceIndexViewModel>
        {
            protected override Expression<Func<ResourcePage, ResourceIndexViewModel>> ToSelector()
            {
                return resource => new ResourceIndexViewModel
                {
                    ResourceId = resource.Id,
                    Title = resource.Title
                };
            }
        }
    }
}