namespace Ubora.Domain.Projects.StructuredInformations.IntendedUsers
{
    public class Nurse : IntendedUser
    {
        public override string Key => "nurse";

        public override string ToDisplayName()
        {
            return "Nurse";
        }
    }
}