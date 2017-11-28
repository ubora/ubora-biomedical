namespace Ubora.Domain.Projects.StructuredInformations.IntendedUsers
{
    public class SelfUserOrPatient : IntendedUser
    {
        public override string Key => "self-user_or_patient";

        public override string ToDisplayName()
        {
            return "Self-user/patient";
        }
    }
}