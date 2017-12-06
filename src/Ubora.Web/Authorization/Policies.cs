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
        public const string CanChangeProjectImage = nameof(CanChangeProjectImage);
        public const string CanEditProjectTitleAndDescription = nameof(CanEditProjectTitleAndDescription);
        public const string CanDeleteProject = nameof(CanDeleteProject);
        public const string CanAddProjectCandidate = nameof(CanAddProjectCandidate);
        public const string CanEditProjectCandidate = nameof(CanEditProjectCandidate);
        public const string CanChangeProjectCandidateImage = nameof(CanChangeProjectCandidateImage);
        public const string CanRemoveProjectCandidateImage = nameof(CanRemoveProjectCandidateImage);
        public const string CanEditComment = nameof(CanEditComment);
        public const string CanVoteCandidate = nameof(CanVoteCandidate);
        public const string CanEditDesignPlanning = nameof(CanEditDesignPlanning);
        public const string CanOpenWorkpackageThree = nameof(CanOpenWorkpackageThree);
    }
}
