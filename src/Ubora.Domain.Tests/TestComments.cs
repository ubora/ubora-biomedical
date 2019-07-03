using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Ubora.Domain.Discussions;

namespace Ubora.Domain.Tests
{
    public static class TestComments
    {
        public static Comment CreateDummy()
        {
            return Comment.Create(
                id: Guid.NewGuid(),
                userId: Guid.NewGuid(),
                text: Guid.NewGuid().ToString(),
                commentedAt: DateTimeOffset.Now,
                additionalData: ImmutableDictionary<string, object>.Empty);
        }
    }
}
