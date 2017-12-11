namespace Ubora.Domain.Projects.StructuredInformations.ProvidersOfMaintenance
{
    public class SelfUserOrPatient: ProviderOfMaintenance
    {
        public override string Key => "self-user_or_patient";
        public override string ToDisplayName()
        {
            return "Self-user/patient";
        }
    }
}
