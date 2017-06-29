﻿using System;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Notifications.Specifications
{
    public class HasArchivedNotifications<T> : WrappedSpecification<T> where T : BaseNotification
    {
        private readonly Guid _userId;

        public HasArchivedNotifications(Guid userId)
        {
            _userId = userId;
        }

        internal override Specification<T> WrapSpecifications()
        {
            return new IsForUser<T>(_userId) && new IsArchived<T>();
        } 
    }

    public class HasArchivedNotifications : HasArchivedNotifications<BaseNotification>
    {
        public HasArchivedNotifications(Guid userId) : base(userId)
        {
        }
    }
}
