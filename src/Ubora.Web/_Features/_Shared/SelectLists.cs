using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ubora.Web._Features._Shared
{
    public static class SelectLists
    {
        public static List<SelectListItem> UserRoles => new List<SelectListItem>(new[]
        {
            new SelectListItem { Value = "I am a...", Text = "I am a...", Disabled = true, Selected = true },
            new SelectListItem { Value = "Developer", Text = "Developer"},
            new SelectListItem { Value = "Mentor", Text = "Mentor"},
            new SelectListItem { Value = "Specialist/Expert", Text = "Specialist/Expert"}
        });

        public static List<SelectListItem> UserMedicalDevices => new List<SelectListItem>(new[]
        {
            new SelectListItem { Value = "Select from the list...", Text = "Select from the list...", Disabled = true, Selected = true },
            new SelectListItem { Value = "Student", Text = "Student"},
            new SelectListItem { Value = "Researcher", Text = "Researcher"},
            new SelectListItem { Value = "Professional Designer", Text = "Professional Designer"},
            new SelectListItem { Value = "Healthcare provider", Text = "Healthcare provider"},
            new SelectListItem { Value = "Consultant", Text = "Consultant"},
            new SelectListItem { Value = "Other", Text = "Other"}
        });

        public static List<SelectListItem> Areas => new List<SelectListItem>(new[]
        {
            new SelectListItem { Value ="Cardiovascular surgery", Text = "Cardiovascular surgery"},
            new SelectListItem { Value ="Colorectal surgery", Text = "Colorectal surgery"},
            new SelectListItem { Value ="General surgery", Text = "General surgery"},
            new SelectListItem { Value ="Neurosurgery", Text = "Neurosurgery"},
            new SelectListItem { Value ="Oncologic surgery", Text = "Oncologic surgery"},
            new SelectListItem { Value ="Ophthalmic surgery", Text = "Ophthalmic surgery"},
            new SelectListItem { Value ="Oral and maxillofacial surgery", Text = "Oral and maxillofacial surgery"},
            new SelectListItem { Value ="Orthopedic surgery", Text = "Orthopedic surgery"},
            new SelectListItem { Value ="Otolaryngology", Text = "Otolaryngology"},
            new SelectListItem { Value ="Pediatric surgery", Text = "Pediatric surgery"},
            new SelectListItem { Value ="Plastic surgery", Text = "Plastic surgery"},
            new SelectListItem { Value ="Podiatric surgery", Text = "Podiatric surgery"},
            new SelectListItem { Value ="Transplant surgery", Text = "Transplant surgery"},
            new SelectListItem { Value ="Trauma surgery", Text = "Trauma surgery"},
            new SelectListItem { Value ="Urology", Text = "Urology"},
            new SelectListItem { Value ="Vascular surgery", Text = "Vascular surgery"},
            new SelectListItem { Value ="Angiology / vascular medicine", Text = "Angiology / vascular medicine"},
            new SelectListItem { Value ="Cardiology", Text = "Cardiology"},
            new SelectListItem { Value ="Critical care medicine", Text = "Critical care medicine"},
            new SelectListItem { Value ="Endocrinology", Text = "Endocrinology"},
            new SelectListItem { Value ="Gastroenterology", Text = "Gastroenterology"},
            new SelectListItem { Value ="Geriatrics", Text = "Geriatrics"},
            new SelectListItem { Value ="Hematology", Text = "Hematology"},
            new SelectListItem { Value ="Hepatology", Text = "Hepatology"},
            new SelectListItem { Value ="Infectious disease", Text = "Infectious disease"},
            new SelectListItem { Value ="Nephrology", Text = "Nephrology"},
            new SelectListItem { Value ="Neurology", Text = "Neurology"},
            new SelectListItem { Value ="Oncology", Text = "Oncology"},
            new SelectListItem { Value ="Pediatrics", Text = "Pediatrics"},
            new SelectListItem { Value ="Pneumology / chest medicine", Text = "Pneumology / chest medicine"},
            new SelectListItem { Value ="Rheumatology", Text = "Rheumatology"},
            new SelectListItem { Value ="Sports medicine", Text = "Sports medicine"},
            new SelectListItem { Value ="Traumatology and orthopedics", Text = "Traumatology and orthopedics"},
            new SelectListItem { Value ="Athology", Text = "Athology"},
            new SelectListItem { Value ="Cellular pathology", Text = "Cellular pathology"},
            new SelectListItem { Value ="Clinical chemistry", Text = "Clinical chemistry"},
            new SelectListItem { Value ="Clinical immunology", Text = "Clinical immunology"},
            new SelectListItem { Value ="Clinical microbiology", Text = "Clinical microbiology"},
            new SelectListItem { Value ="Clinical neurophysiology", Text = "Clinical neurophysiology"},
            new SelectListItem { Value ="Diagnostic radiology", Text = "Diagnostic radiology"},
            new SelectListItem { Value ="Hematology", Text = "Hematology"},
            new SelectListItem { Value ="Molecular biology", Text = "Molecular biology"},
            new SelectListItem { Value ="Nuclear medicine", Text = "Nuclear medicine"},
            new SelectListItem { Value ="Transfusion medicine", Text = "Transfusion medicine"},
            new SelectListItem { Value ="Anesthesiology", Text = "Anesthesiology"},
            new SelectListItem { Value ="Dermatology", Text = "Dermatology"},
            new SelectListItem { Value ="Emergency medicine", Text = "Emergency medicine"},
            new SelectListItem { Value ="Family medicine", Text = "Family medicine"},
            new SelectListItem { Value ="Gerontology", Text = "Gerontology"},
            new SelectListItem { Value ="Medical genetics", Text = "Medical genetics"},
            new SelectListItem { Value ="Neurology", Text = "Neurology"},
            new SelectListItem { Value ="Obstetrics and gynecology", Text = "Obstetrics and gynecology"},
            new SelectListItem { Value ="Ophthalmology", Text = "Ophthalmology"},
            new SelectListItem { Value ="Pediatrics", Text = "Pediatrics"},
            new SelectListItem { Value ="Pharmaceutical medicine", Text = "Pharmaceutical medicine"},
            new SelectListItem { Value ="Preventive medicine", Text = "Preventive medicine"},
            new SelectListItem { Value ="Psychiatry", Text = "Psychiatry"},
            new SelectListItem { Value ="Public health", Text = "Public health"},
            new SelectListItem { Value ="Rehabilitation", Text = "Rehabilitation"},
        });

        public static List<SelectListItem> ClinicalNeedTags => new List<SelectListItem>(new[]
        {
            new SelectListItem { Value ="Monitoring purpose", Text = "Monitoring purpose" },
            new SelectListItem { Value ="Non-surgical therapy / Administration of drugs", Text = "Non-surgical therapy / Administration of drugs" },
            new SelectListItem { Value ="Point-of-care diagnosis", Text = "Point-of-care diagnosis" },
            new SelectListItem { Value ="Prevention of pathology or disease", Text = "Prevention of pathology or disease" },
            new SelectListItem { Value ="Rehabilitation", Text = "Rehabilitation" },
            new SelectListItem { Value ="Remote or self-diagnosis", Text = "Remote or self-diagnosis" },
            new SelectListItem { Value ="Replacement of human tissues or organs", Text = "Replacement of human tissues or organs" },
            new SelectListItem { Value ="Support to laboratory practice", Text = "Support to laboratory practice" },
            new SelectListItem { Value ="Support to medical practice", Text = "Support to medical practice" },
            new SelectListItem { Value ="Support to surgery", Text = "Support to surgery" },
        });

        public static List<SelectListItem> PotentialTechnologyTags => new List<SelectListItem>(new[]
        {
            new SelectListItem { Value ="Active implantable device", Text = "Active implantable device"},
            new SelectListItem { Value ="E-based technology", Text = "E-based technology"},
            new SelectListItem { Value ="Ergonomic support", Text = "Ergonomic support"},
            new SelectListItem { Value ="Implantable device", Text = "Implantable device"},
            new SelectListItem { Value ="In vitro diagnostic device", Text = "In vitro diagnostic device"},
            new SelectListItem { Value ="In vivo diagnostic device", Text = "In vivo diagnostic device"},
            new SelectListItem { Value ="Laboratory equipment", Text = "Laboratory equipment"},
            new SelectListItem { Value ="Mobile-based technology", Text = "Mobile-based technology"},
            new SelectListItem { Value ="Monitoring device", Text = "Monitoring device"},
            new SelectListItem { Value ="Other supporting equipment", Text = "Other supporting equipment"},
            new SelectListItem { Value ="Preventive device", Text = "Preventive device"},
            new SelectListItem { Value ="Software", Text = "Software"},
            new SelectListItem { Value ="Surgical device", Text = "Surgical device"},
        });
    }
}
