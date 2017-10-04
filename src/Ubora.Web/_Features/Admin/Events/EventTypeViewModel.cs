using System.Collections.Generic;
using System.Reflection;

namespace Ubora.Web._Features.Admin.Events
{
    public class EventsViewModel
    {
        public EventFilter Filter { get; set; }
        public IEnumerable<EventInfoViewModel> Events { get; set; }

        public class EventInfoViewModel
        {
            public TypeInfo Info { get; set; }
            public List<PropertyInfo> PropertyInfos { get; set; } = new List<PropertyInfo>();
        }
    }
}