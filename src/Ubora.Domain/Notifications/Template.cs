using System;

namespace Ubora.Domain.Notifications
{
    public static class Template
    {
        public static string User(Guid userId)
        {
            return userId.ToString();
        }

        public static string Project(Guid projectId)
        {
            return projectId.ToString();
        }
    }
}