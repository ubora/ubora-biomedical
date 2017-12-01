using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;

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

        [JsonIgnore]
        public string EnglishName
        {
            get
            {
                if (string.IsNullOrEmpty(Code))
                {
                    return "";
                }

                var regionInfos = GetRegionInfos();
                var countryNames = regionInfos.Where(regionInfo => regionInfo.ThreeLetterISORegionName == Code.ToUpper())
                    .ToList();

                return countryNames.FirstOrDefault() != null
                    ? countryNames.First().EnglishName
                    : Code;
            }
        }

        public static IEnumerable<Country> GetAllCountries()
        {
            var regionInfos = GetRegionInfos().Distinct();

            var countries = regionInfos.Where(regionInfo => !string.IsNullOrEmpty(regionInfo.ThreeLetterISORegionName))
                .Select(regionInfo => new Country(regionInfo.ThreeLetterISORegionName));

            return countries;
        }

        private static IEnumerable<RegionInfo> GetRegionInfos()
        {
            var cultureInfos = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            var regionInfos = cultureInfos.Select(cultureInfo => new RegionInfo(cultureInfo.Name));
            return regionInfos;
        }
    }
}
