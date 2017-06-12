namespace Ubora.Web.Authorization
{
    public class Policies
    {
        public const string IsProjectMember = nameof(IsProjectMember);
        public const string IsAuthenticatedUser = nameof(IsAuthenticatedUser);
        public const string CanRemoveProjectMember = nameof(CanRemoveProjectMember);
    }
}
