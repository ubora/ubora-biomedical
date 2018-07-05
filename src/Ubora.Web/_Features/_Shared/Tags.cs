using System.Collections.Immutable;

namespace Ubora.Web._Features._Shared
{
    public static class Tags
    {
        public static readonly ImmutableList<string> SurgicalFieldAreas =
            new[]
            {
                "Cardiovascular surgery",
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
                "Vascular surgery"
            }.ToImmutableList();

        public static readonly ImmutableList<string> InternalMedicineAreas = 
            new[]
            {
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
                "Traumatology and orthopedics"
            }.ToImmutableList();

        public static readonly ImmutableList<string> DiagnosticMedicineAreas =
            new[]
            {
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
                "Transfusion medicine"
            }.ToImmutableList();

        public static readonly ImmutableList<string> OtherMajorDisciplineAreas =
            new[]
            {
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
            }.ToImmutableList();

        public static readonly ImmutableList<string> Areas = SurgicalFieldAreas.AddRange(InternalMedicineAreas).AddRange(DiagnosticMedicineAreas).AddRange(OtherMajorDisciplineAreas);
        
        public static readonly ImmutableList<string> ClinicalNeeds = new []
        {
            "Monitoring purpose",
            "Non-surgical therapy / Administration of drugs",
            "Point-of-care diagnosis",
            "Prevention of pathology or disease",
            "Rehabilitation",
            "Remote or self-diagnosis",
            "Replacement of human tissues or organs",
            "Support to laboratory practice", 
            "Support to medical practice", 
            "Support to surgery"
        }.ToImmutableList();
        
        public static readonly ImmutableList<string> PotentialTechnologies = new []
        {
            "Active implantable device",
            "E-based technology",
            "Ergonomic support",
            "Implantable device",
            "In vitro diagnostic device",
            "In vivo diagnostic device",
            "Laboratory equipment",
            "Mobile-based technology",
            "Monitoring device",
            "Other supporting equipment",
            "Preventive device",
            "Software", 
            "Surgical device"
        }.ToImmutableList();
    }
}