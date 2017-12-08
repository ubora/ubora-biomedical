namespace Ubora.Domain.Projects.StructuredInformations.TypesOfUse
{
    public class SingleUse : TypeOfUse
    {
        public override string Key => "single_use";
        public override string ToDisplayName()
        {
            return "Single use";
        }
    }
}
