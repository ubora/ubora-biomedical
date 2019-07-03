using System;
using System.Collections.Generic;
using Ubora.Domain.Resources;

namespace Ubora.Web._Areas.ResourcesArea.ResourceCategories
{
    public class FormSelectOptionsViewModel
    {
        public IReadOnlyCollection<ResourceCategory> Categories { get; set; }
        public Guid? SelectedCategory { get; set; }
        public Guid? RemovedCategory { get; set; }
    }
}
