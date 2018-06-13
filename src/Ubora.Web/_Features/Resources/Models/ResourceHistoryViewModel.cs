using System;
using System.Collections.Generic;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Web._Features.Resources.Models
{
    public class ResourceHistoryViewModel
    {
        public Guid ResourceId { get; set; }
        public string Title { get; set; }
        public IReadOnlyCollection<UboraEvent> Events { get; set; }
    }
}