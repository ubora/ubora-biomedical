namespace Ubora.Domain.Projects.StructuredInformations.ProvidersOfMaintenance
{
    public class Manufacturer : ProviderOfMaintenance
    {
        public override string Key => "Manufacturer";
        public override string ToDisplayName()
        {
            return "Manufacturer";
        }
    }
}
