﻿namespace Ubora.Web.Authorization
{
    public class Policies
    {
        public const string ProjectController = nameof(ProjectController);
        public const string IsAuthenticatedUser = nameof(IsAuthenticatedUser);
        public const string CanRemoveProjectMember = nameof(CanRemoveProjectMember);
    }
}
