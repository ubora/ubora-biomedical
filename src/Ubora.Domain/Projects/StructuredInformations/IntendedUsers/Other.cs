namespace Ubora.Domain.Projects.StructuredInformations.IntendedUsers
{
    public class Other : IntendedUser
    {
        public Other(string description)
        {
            Description = description;
        }

        public string Description { get; }

        public override string Key => "other";

        public override string ToDisplayName()
        {
            return Description;
        }
    }
}