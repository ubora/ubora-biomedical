namespace Ubora.Domain.Projects.StructuredInformations.TypesOfUse
{
    public class CapitalEquipment : TypeOfUse
    {
        public override string Key => "capital_equipment";
        public override string ToDisplayName()
        {
            return "Capital equipment";
        }
    }
}
