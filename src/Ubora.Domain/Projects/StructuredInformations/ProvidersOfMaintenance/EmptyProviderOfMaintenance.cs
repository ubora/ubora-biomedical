namespace Ubora.Domain.Projects.StructuredInformations.ProvidersOfMaintenance
{
    public class EmptyProviderOfMaintenance : ProviderOfMaintenance
    {
        public override string Key => "empty";
        public override string ToDisplayName()
        {
            return "";
        }
    }
}
