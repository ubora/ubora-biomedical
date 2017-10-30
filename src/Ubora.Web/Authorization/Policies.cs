namespace Ubora.Web.Authorization
{
    public class Policies
    {
        public const string ProjectController = nameof(ProjectController);
        public const string IsAuthenticatedUser = nameof(IsAuthenticatedUser);
        public const string CanRemoveProjectMember = nameof(CanRemoveProjectMember);
        public const string CanReviewProjectWorkpackages = nameof(CanReviewProjectWorkpackages);
        public const string CanSubmitWorkpackageForReview = nameof(CanSubmitWorkpackageForReview);
        public const string CanEditWorkpackageOne = nameof(CanEditWorkpackageOne);
        public const string CanHideProjectFile = nameof(CanHideProjectFile);
        public const string CanCreateProject = nameof(CanCreateProject);
        public const string CanJoinProject = nameof(CanJoinProject);
    }
}
