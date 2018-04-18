namespace Ubora.Domain.Projects.StructuredInformations.Portabilities
{
    public class InstalledAndStationary : Portability
    {
        public override string Key => "installed_and_stationary";
        public override string ToDisplayName()
        {
            return "Installed and stationary";
        }
    }
}
