using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ubora.Web._Features.Projects.Workpackages.Steps.IsoCompliances.Models
{
    public class AddIsoStandardPostModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string ShortDescription { get; set; }

        [Url]
        [Required]
        public string Link { get; set; }
    }
}
