﻿using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Resources.Events;

namespace Ubora.Domain.Resources
{
    public class ResourcePage : Entity<ResourcePage>
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public int MenuPriority { get; private set; }
        public Guid? CategoryId { get; private set; }

        public int BodyVersion { get; private set; }
        public QuillDelta Body { get; private set; }

        public string GetBlobContainerName() => $"resourcepage-{Id}";

        private void SetBody(QuillDelta body)
        {
            BodyVersion++;
            Body = body;
        }

        private void Apply(ResourcePageCreatedEvent @event)
        {
            if (@event.ResourcePageId == default(Guid))
                throw new ArgumentException(nameof(@event.ResourcePageId));

            Id = @event.ResourcePageId;
            Title = @event.Title;
            MenuPriority = @event.MenuPriority;
            CategoryId = @event.ParentCategoryId;

            SetBody(@event.Body);
        }

        private void Apply(ResourcePageTitleChangedEvent @event)
        {
            Title = @event.Title;
        }

        private void Apply(ResourcePageBodyEditedEvent @event)
        {
            if (BodyVersion != @event.PreviousBodyVersion)
                throw new InvalidOperationException("Content has been changed -- the versions don't match.");

            SetBody(@event.Body);
        }

        private void Apply(ResourcePageMenuPreferencesChangedEvent @event)
        {
            CategoryId = @event.ParentCategoryId;
            MenuPriority = @event.MenuPriority;
        }
    }
}