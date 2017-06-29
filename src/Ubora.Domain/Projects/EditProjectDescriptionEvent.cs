﻿using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects
{
    public class EditProjectDescriptionEvent : UboraEvent
    {
        public Guid Id { get; set; }
        public string Description { get; set; }

        public EditProjectDescriptionEvent(UserInfo initiatedBy) : base(initiatedBy)
        {
        }

        public override string GetDescription() => "Updated project description";
    }
}