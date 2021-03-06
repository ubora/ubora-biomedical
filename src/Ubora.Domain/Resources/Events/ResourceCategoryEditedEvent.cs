﻿using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Resources.Events
{
    public class ResourceCategoryEditedEvent : UboraEvent
    {
        public ResourceCategoryEditedEvent(UserInfo initiatedBy, Guid categoryId, string title, string description, Guid? parentCategoryId, int menuPriority)
            : base(initiatedBy)
        {
            CategoryId = categoryId;
            Title = title;
            Description = description;
            ParentCategoryId = parentCategoryId;
            MenuPriority = menuPriority;
        }

        public Guid CategoryId { get; }
        public string Title { get; }
        public string Description { get; }
        public Guid? ParentCategoryId { get; }
        public int MenuPriority { get; }

        public override string GetDescription() => "created resource category.";
    }
}