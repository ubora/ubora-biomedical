using System.Collections.Generic;

namespace Ubora.Web._Features.ProjectList.Models
{
    public class SearchViewModel
    {
        public string Title { get; set; }
        public TabType Tab { get; set; }
        public string ByArea { get; set; }
        public ByStatusFilteringMethod ByStatus { get; set; }
        public SortBy SortBy { get; set; }
        public ProjectListViewModel ProjectListViewModel { get; set; }
        public List<string> AreaOfUsageTags { get; set; } =
            new List<string> { "Cardiovascular surgery",
                                "Colorectal surgery",
                                "General surgery",
                                "Neurosurgery",
                                "Oncologic surgery",
                                "Ophthalmic surgery",
                                "Oral and maxillofacial surgery",
                                "Orthopedic surgery",
                                "Otolaryngology",
                                "Pediatric surgery",
                                "Plastic surgery",
                                "Podiatric surgery",
                                "Transplant surgery",
                                "Trauma surgery",
                                "Urology",
                                "Vascular surgery",

                                "Angiology / vascular medicine",
                                "Cardiology",
                                "Critical care medicine",
                                "Endocrinology",
                                "Gastroenterology",
                                "Geriatrics",
                                "Hematology",
                                "Hepatology",
                                "Infectious disease",
                                "Nephrology",
                                "Neurology",
                                "Oncology",
                                "Pediatrics",
                                "Pneumology / chest medicine",
                                "Rheumatology",
                                "Sports medicine",
                                "Traumatology and orthopedics",

                                "Athology",
                                "Cellular pathology",
                                "Clinical chemistry",
                                "Clinical immunology",
                                "Clinical microbiology",
                                "Clinical neurophysiology",
                                "Diagnostic radiology",
                                "Hematology",
                                "Molecular biology",
                                "Nuclear medicine",
                                "Transfusion medicine",

                                "Anesthesiology",
                                "Dermatology",
                                "Emergency medicine",
                                "Family medicine",
                                "Gerontology",
                                "Medical genetics",
                                "Neurology",
                                "Obstetrics and gynecology",
                                "Ophthalmology",
                                "Pediatrics",
                                "Pharmaceutical medicine",
                                "Preventive medicine",
                                "Psychiatry",
                                "Public health",
                                "Rehabilitation"
                            };
    }
}
