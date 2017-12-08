namespace Ubora.Domain.Projects.StructuredInformations.Portabilities
{
    public class Portable : Portability
    {
        public override string Key => "portable";
        public override string ToDisplayName()
        {
            return "Portable";
        }
    }
}
