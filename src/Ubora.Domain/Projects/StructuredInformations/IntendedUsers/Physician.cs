namespace Ubora.Domain.Projects.StructuredInformations.IntendedUsers
{
    public class Physician : IntendedUser
    {
        public override string Key => "physician";

        public override string ToDisplayName()
        {
            return "Physician";
        }
    }
}