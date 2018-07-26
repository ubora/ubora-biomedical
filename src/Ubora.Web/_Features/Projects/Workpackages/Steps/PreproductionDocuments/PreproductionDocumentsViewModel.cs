using System.Collections.Generic;

namespace Ubora.Web._Features.Projects.Workpackages.Steps.PreproductionDocuments
{
    public class PreproductionDocumentsViewModel
    {
        public List<WorkpackageSelectList> WorkpackageSelectList { get; set; }
    }

    public class WorkpackageSelectList
    {
        public string Name { get; set; }
        public bool Checked { get; set; }
    }
}