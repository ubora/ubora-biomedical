﻿using Marten.Schema;
using System;

namespace Ubora.Domain.Notifications
{
    public abstract class BaseNotification
    {
        [Identity]
        public Guid Id { get; }
        public Guid NotificationTo { get; private set; }
        public bool HasBeenViewed { get; internal set; }
        public abstract bool IsArchived { get; }
        public abstract bool IsPending { get; }

        public BaseNotification(Guid id, Guid inviteTo)
        {
            Id = id;
            NotificationTo = inviteTo;
        }
    }
}