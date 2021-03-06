﻿using System.Collections.Generic;

namespace Ubora.Web._Features.Projects.Workpackages.Steps.PreproductionDocuments
{
    public class PreproductionDocumentsViewModel
    {
        public List<WorkpackageCheckBoxListItem> WorkpackageCheckBoxListItems { get; set; }
    }

    public class WorkpackageCheckBoxListItem
    {
        public string Name { get; set; } 
        public bool IsExportable { get; set; }
    }
}