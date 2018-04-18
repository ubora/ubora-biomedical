namespace Ubora.Domain.Projects.StructuredInformations.ProvidersOfMaintenance
{
    public class OtherProviderOfMaintenance : ProviderOfMaintenance
    {
        public OtherProviderOfMaintenance(string description)
        {
            Description = description;
        }

        public string Description { get; }

        public override string Key => "other";

        public override string ToDisplayName()
        {
            return Description;
        }
    }
}
