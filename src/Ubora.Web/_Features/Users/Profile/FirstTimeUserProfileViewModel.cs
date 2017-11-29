using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ubora.Domain.Users;

namespace Ubora.Web._Features.Users.Profile
{
    public class FirstTimeUserProfileViewModel
    {
        public string Biography { get; set; }
        public string Degree { get; set; }
        public string Field { get; set; }
        public string University { get; set; }
        public string MedicalDevice { get; set; }
        public string Institution { get; set; }
        public string Skills { get; set; }
        public string Role { get; set; }
        public string CountryCode { get; set; }
        public IEnumerable<SelectListItem> Countries
        {
            get
            {
                var allCountries = Country.GetAllCountries()
                    .OrderBy(x => x.EnglishName);

                var countries =  allCountries.Select(country => new SelectListItem
                {
                    Value = country.Code,
                    Text = country.EnglishName
                });

                return countries;
            }
        }
    }
}
