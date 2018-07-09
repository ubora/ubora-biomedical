﻿using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Resources.Events
{
    public class ResourcePageCreatedEvent : UboraEvent
    {
        public ResourcePageCreatedEvent(UserInfo initiatedBy, Guid resourcePageId, Slug slug, string title, QuillDelta body, int menuPriority, Guid? parentCategoryId)
            : base(initiatedBy)
        {
            ResourcePageId = resourcePageId;
            Slug = slug;
            Title = title;
            Body = body;
            MenuPriority = menuPriority;
            ParentCategoryId = parentCategoryId;
        }

        public Guid ResourcePageId { get; }
        public Slug Slug { get; }
        public string Title { get; }
        public QuillDelta Body { get; }
        public int MenuPriority { get; }
        public Guid? ParentCategoryId { get; }

        public override string GetDescription() => "created resource page.";
    }
}