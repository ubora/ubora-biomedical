namespace Ubora.Domain.Projects.StructuredInformations.TypesOfUse
{
    public class Reusable : TypeOfUse
    {
        public override string Key => "reusable";
        public override string ToDisplayName()
        {
            return "Reusable";
        }
    }
}
