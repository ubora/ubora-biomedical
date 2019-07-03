using System;

namespace Ubora.Domain.Tests
{
    public static class TestStrings
    {
        public static string CreateRandom()
        {
            return $@"rAnd0m $tr1ng
{Guid.NewGuid()}
";
        }
    }
}