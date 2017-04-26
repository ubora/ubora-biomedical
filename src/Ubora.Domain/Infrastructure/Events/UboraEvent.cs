﻿using System;

namespace Ubora.Domain.Infrastructure.Events
{
    public abstract class UboraEvent
    {
        protected UboraEvent(UserInfo creator)
        {
            Creator = creator ?? throw new ArgumentNullException(nameof(creator));
        }
        public UserInfo Creator { get; }

        public abstract string GetDescription();

        public override string ToString()
        {
            return $"\"{Creator.Name}\": {GetDescription()}";
        }
    }
}
