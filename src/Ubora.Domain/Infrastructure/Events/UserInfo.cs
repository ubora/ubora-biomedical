using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Ubora.Domain.Infrastructure.Events
{
    public class UserInfo : ValueObject
    {
        public UserInfo(Guid userId, string name)
        {
            UserId = userId;
            Name = name;
        }

        public Guid UserId { get; private set; }
        public string Name { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return UserId;
            yield return Name;
        }
    }
}