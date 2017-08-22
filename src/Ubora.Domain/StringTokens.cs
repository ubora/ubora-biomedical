using System;

namespace Ubora.Domain
{
    public static class StringTokens
    {
        public static string User(Guid userId)
        {
            return $"#user{{{userId}}}";
        }

        public static string Project(Guid projectId)
        {
            return $"#project{{{projectId}}}";
        }
    }
}