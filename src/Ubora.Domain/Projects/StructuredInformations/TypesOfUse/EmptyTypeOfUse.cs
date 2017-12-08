namespace Ubora.Domain.Projects.StructuredInformations.TypesOfUse
{
    public class EmptyTypeOfUse : TypeOfUse
    {
        public override string Key => "empty";
        public override string ToDisplayName()
        {
            return "";
        }
    }
}
