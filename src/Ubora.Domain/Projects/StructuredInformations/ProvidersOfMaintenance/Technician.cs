namespace Ubora.Domain.Projects.StructuredInformations.ProvidersOfMaintenance
{
    public class Technician : ProviderOfMaintenance
    {
        public override string Key => "technician";
        public override string ToDisplayName()
        {
            return "Technician";
        }
    }
}
