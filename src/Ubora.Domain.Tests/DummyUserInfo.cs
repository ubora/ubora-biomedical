using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Tests
{
    public class DummyUserInfo : UserInfo
    {
        private static readonly Guid Id = Guid.NewGuid();

        public DummyUserInfo() : base(Id, Id.ToString())
        {
        }
    }
}
