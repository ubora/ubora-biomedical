namespace Ubora.Domain.Projects.StructuredInformations.IntendedUsers
{
    public class SelfUseOrPatient : IntendedUser
    {
        public override string Key => "self-use_or_patient";

        public override string ToDisplayName()
        {
            return "Self-use/patient";
        }
    }
}