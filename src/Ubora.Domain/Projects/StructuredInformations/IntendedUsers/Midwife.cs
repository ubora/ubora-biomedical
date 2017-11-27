namespace Ubora.Domain.Projects.StructuredInformations.IntendedUsers
{
    public class Midwife : IntendedUser
    {
        public override string Key => "midwife";

        public override string ToDisplayName()
        {
            return "Midwife";
        }
    }
}