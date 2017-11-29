using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Ubora.Domain.Users
{
    public class Country
    {
        /// <summary>
        /// Three-letter country code
        /// </summary>
        public string Code { get; private set; }

        public Country(string code)
        {
            Code = code;
        }

        public string EnglishName
        {
            get
            {
                if (string.IsNullOrEmpty(Code))
                {
                    return "";
                }

                var cultureInfos = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

                var regionInfos = cultureInfos.Select(cultureInfo => new RegionInfo(cultureInfo.Name));
                var countryNames = regionInfos.Where(regionInfo => regionInfo.ThreeLetterISORegionName == Code.ToUpper());

                return countryNames.FirstOrDefault()?.EnglishName;
            }
        }

        public static IEnumerable<Country> GetAllCountries()
        {
            var cultureInfos = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            var regionInfos = cultureInfos.Select(cultureInfo => new RegionInfo(cultureInfo.Name)).Distinct();

            var countries = regionInfos.Where(regionInfo => !string.IsNullOrEmpty(regionInfo.ThreeLetterISORegionName))
                .Select(regionInfo => new Country(regionInfo.ThreeLetterISORegionName));

            return countries;
        }
    }
}
