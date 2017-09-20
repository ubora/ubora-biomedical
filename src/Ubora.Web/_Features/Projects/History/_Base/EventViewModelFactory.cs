using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Web._Features.Projects.History._Base
{
    public abstract class EventViewModelFactory<TEvent, TViewModel> : IEventViewModelFactory
        where TEvent : UboraEvent
        where TViewModel : IEventViewModel<TEvent>
    {
        public bool CanCreateFor(Type type)
        {
            return type == typeof(TEvent);
        }

        public abstract TViewModel Create(TEvent uboraEvent, DateTimeOffset timestamp);
    }
}
