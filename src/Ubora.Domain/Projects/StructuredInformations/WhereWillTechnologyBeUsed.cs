using Newtonsoft.Json;

namespace Ubora.Domain.Projects.StructuredInformations
{
    public class WhereWillTechnologyBeUsed
    {
        public WhereWillTechnologyBeUsed(
            bool ruralSettings,
            bool urbanSettings,
            bool outdoors,
            bool indoors,
            bool atHome,
            bool primaryLevel,
            bool secondaryLevel,
            bool tertiaryLevel)
        {
            RuralSettings = ruralSettings;
            UrbanSettings = urbanSettings;
            Outdoors = outdoors;
            Indoors = indoors;
            AtHome = atHome;
            PrimaryLevel = primaryLevel;
            SecondaryLevel = secondaryLevel;
            TertiaryLevel = tertiaryLevel;
        }

        [JsonConstructor]
        private WhereWillTechnologyBeUsed()
        {
        }

        public bool RuralSettings { get; private set; }
        public bool UrbanSettings { get; private set; }
        public bool Outdoors { get; private set; }
        public bool Indoors { get; private set; }
        public bool AtHome { get; private set; }
        // (health post, health center)
        public bool PrimaryLevel { get; private set; }
        // (general hospital)
        public bool SecondaryLevel { get; private set; }
        // (specialist hospital)
        public bool TertiaryLevel { get; private set; }

        public static WhereWillTechnologyBeUsed CreateEmpty()
        {
            return new WhereWillTechnologyBeUsed();
        }
    }
}