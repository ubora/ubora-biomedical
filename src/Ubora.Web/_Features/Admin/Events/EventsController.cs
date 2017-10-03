using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Web.Data;

namespace Ubora.Web._Features.Admin.Events
{
    [Authorize(Roles = ApplicationRole.Admin)]
    public class EventsController : UboraController
    {
        [Route("Admin/Events")]
        public IActionResult Events(EventFilter filter = EventFilter.Namespace)
        {
            var events = DomainAutofacModule.FindDomainEventConcreteTypes();

            var eventViewModels = GetEventViewModels(events);

            var model = new EventsViewModel
            {
                Events = eventViewModels,
                Filter = filter
            };

            return View(model);
        }

        private IEnumerable<EventsViewModel.EventInfoViewModel> GetEventViewModels(IEnumerable<Type> events)
        {
            foreach (var @event in events)
            {
                var typeInfo = @event.GetTypeInfo();

                var model = new EventsViewModel.EventInfoViewModel
                {
                    Info = typeInfo
                };

                do
                {
                    foreach (var property in typeInfo.DeclaredProperties)
                    {
                        model.PropertyInfos.Add(property);
                    }

                    typeInfo = typeInfo.BaseType.GetTypeInfo();

                } while (typeInfo.AsType() != typeof(object));

                yield return model;
            }
        }
    }
}