namespace Ubora.Domain.Projects.StructuredInformations.ProvidersOfMaintenance
{
    public class Engineer : ProviderOfMaintenance
    {
        public override string Key => "engineer";
        public override string ToDisplayName()
        {
            return "Engineer";
        }
    }
}
