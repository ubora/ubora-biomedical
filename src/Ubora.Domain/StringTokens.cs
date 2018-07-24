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

        public static string WorkpackageOneReview(Guid projectId)
        {
            return $"#review1{{{projectId}}}";
        }

        public static string WorkpackageTwoReview(Guid projectId)
        {
            return $"#review2{{{projectId}}}";
        }

        public static string Task(Guid taskId)
        {
            return $"#task{{{taskId}}}";
        }

        public static string Candidate(Guid candidateId)
        {
            return $"#candidate{{{candidateId}}}";
        }
    }
}