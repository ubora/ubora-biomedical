using System;
using System.Collections.Generic;
using Ubora.Web._Features._Shared.Paging;
using Ubora.Web._Features.Projects.History._Base;

namespace Ubora.Web._Features.Resources.Models
{
    public class ResourceHistoryViewModel
    {
        public Guid ResourceId { get; set; }
        public string Title { get; set; }
        public IReadOnlyCollection<IEventViewModel> Events { get; set; }
    }
}