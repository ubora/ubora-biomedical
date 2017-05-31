using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Tests
{
    public class DummyUserInfo : UserInfo
    {
        public DummyUserInfo() : base(Guid.NewGuid(), nameof(DummyUserInfo))
        {
        }
    }
}
