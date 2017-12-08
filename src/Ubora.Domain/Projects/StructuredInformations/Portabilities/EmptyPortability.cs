namespace Ubora.Domain.Projects.StructuredInformations.Portabilities
{
    public class EmptyPortability : Portability
    {
        public override string Key => "empty";
        public override string ToDisplayName()
        {
            return "";
        }
    }
}
