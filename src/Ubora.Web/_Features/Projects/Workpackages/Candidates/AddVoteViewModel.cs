using System;
using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Features.Projects.Workpackages.Candidates
{
    public class AddVoteViewModel
    {
        public Guid CandidateId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Value for functionality must be between {1} and {2}.")]
        public int Functionality { get; set; }
        [Required]
        [Range(1, 5, ErrorMessage = "Value for performance must be between {1} and {2}.")]
        public int Performace { get; set; }
        [Required]
        [Range(1, 5, ErrorMessage = "Value for usability must be between {1} and {2}.")]
        public int Usability { get; set; }
        [Required]
        [Range(1, 5, ErrorMessage = "Value for safety must be between {1} and {2}.")]
        public int Safety { get; set; }
    }
}