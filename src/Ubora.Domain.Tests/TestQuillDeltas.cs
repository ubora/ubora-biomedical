using System;

namespace Ubora.Domain.Tests
{
    public static class TestQuillDeltas
    {
        public static QuillDelta CreateRandom()
        {
            return new QuillDelta($"{{\"ops\":[{{\"insert\":\"{Guid.NewGuid()}\\n\"}}]}}");
        }
    }
}
