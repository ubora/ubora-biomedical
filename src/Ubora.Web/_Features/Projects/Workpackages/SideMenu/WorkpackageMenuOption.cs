namespace Ubora.Web._Features.Projects.Workpackages
{
    public static class WorkpackageMenuOption
    {
        public static string DeviceClassification => "DeviceClassification";
        public static string RegulationChecklist => "RegulationChecklist";
        public static string Wp1MentorReview => "WorkpackageOneReview";
        public static string Voting => "Voting";
        public static string DesignPlanning => "DesignPlanning";
        public static string Step(string stepId) => stepId;
    }
}
