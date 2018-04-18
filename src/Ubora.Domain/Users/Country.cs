using System;
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
            if (code == null)
            {
                throw new ArgumentNullException(nameof(code));
            }
            var isKnownCountryCode = GetRegionInfos().Any(regionInfo => regionInfo.ThreeLetterISORegionName == code.ToUpper());
            if (!isKnownCountryCode)
            {
                throw new InvalidOperationException();
            }
            Code = code.ToUpper();
        }

        public static Country CreateEmpty()
        {
            return new Country { Code = "", _displayName = "" };
        }

        [JsonConstructor]
        protected Country()
        {
        }

        [JsonProperty(nameof(DisplayName))]
        private string _displayName;

        [JsonIgnore]
        public string DisplayName
        {
            get
            {
                if (_displayName == null)
                {
                    _displayName = FindDisplayNameForFirstTime();
                }
                return _displayName;
            }
        }

        public static IEnumerable<Country> GetAllCountries()
        {
            var regionInfos = GetRegionInfos().Distinct();

            var countries = regionInfos.Where(regionInfo => !string.IsNullOrEmpty(regionInfo.ThreeLetterISORegionName))
                .Select(regionInfo => new Country
                {
                    _displayName = regionInfo.EnglishName,
                    Code = regionInfo.ThreeLetterISORegionName
                });

            return countries;
        }

        private static IEnumerable<RegionInfo> GetRegionInfos()
        {
            var cultureInfos = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            var regionInfos = cultureInfos.Select(cultureInfo => new RegionInfo(cultureInfo.Name));
            return regionInfos;
        }

        private string FindDisplayNameForFirstTime()
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
}
