namespace Ubora.Domain.Projects.StructuredInformations.IntendedUsers
{
    public class Technician : IntendedUser
    {
        public override string Key => "technician";

        public override string ToDisplayName()
        {
            return "Technician";
        }
    }
}