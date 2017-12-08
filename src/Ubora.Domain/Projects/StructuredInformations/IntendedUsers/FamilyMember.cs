namespace Ubora.Domain.Projects.StructuredInformations.IntendedUsers
{
    public class FamilyMember : IntendedUser
    {
        public override string Key => "family_member";

        public override string ToDisplayName()
        {
            return "Family member";
        }
    }
}