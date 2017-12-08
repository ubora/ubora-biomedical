namespace Ubora.Domain.Projects.StructuredInformations.ProvidersOfMaintenance
{
    public class NurseOrPhysician : ProviderOfMaintenance
    {
        public override string Key => "nurse_or_physician";
        public override string ToDisplayName()
        {
            return "Nurse/Physician";
        }
    }
}
