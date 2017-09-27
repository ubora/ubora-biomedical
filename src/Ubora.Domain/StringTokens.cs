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

        public static string File(Guid fileId)
        {
            return $"#file{{{fileId}}}";
        }

        public static string WorkpackageOneReview()
        {
            return "#{review1}";
        }

        public static string WorkpackageTwoReview()
        {
            return "#{review2}";
        }

        public static string Task(Guid taskId)
        {
            return $"#task{{{taskId}}}";
        }
    }
}