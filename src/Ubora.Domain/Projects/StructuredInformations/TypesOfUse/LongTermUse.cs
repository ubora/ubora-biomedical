namespace Ubora.Domain.Projects.StructuredInformations.TypesOfUse
{
    public class LongTermUse : TypeOfUse
    {
        public override string Key => "long_term_use";
        public override string ToDisplayName()
        {
            return "Long term use";
        }
    }
}
