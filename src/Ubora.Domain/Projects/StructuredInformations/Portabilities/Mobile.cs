namespace Ubora.Domain.Projects.StructuredInformations.Portabilities
{
    public class Mobile : Portability
    {
        public override string Key => "mobile";
        public override string ToDisplayName()
        {
            return "Mobile";
        }
    }
}
