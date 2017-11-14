using System;
using System.Collections.Generic;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Web._Features.Projects.History._Base
{
    public interface IEventViewModelFactoryMediator
    {
        IEventViewModel Create(UboraEvent uboraEvent, DateTimeOffset timeStamp);
    }

    public class EventViewModelFactoryMediator : IEventViewModelFactoryMediator
    {
        private readonly IEnumerable<IEventViewModelFactory> _concreteFactories;
        private readonly GeneralEventViewModel.Factory _generalFactory;

        public EventViewModelFactoryMediator(IEnumerable<IEventViewModelFactory> concreteFactories, 
            GeneralEventViewModel.Factory generalFactory)
        {
            _generalFactory = generalFactory;
            _concreteFactories = concreteFactories;
        }

        protected EventViewModelFactoryMediator()
        {
        }

        public virtual IEventViewModel Create(UboraEvent uboraEvent, DateTimeOffset timeStamp)
        {
            var eventType = uboraEvent.GetType();

            foreach (var factory in _concreteFactories)
            {
                if (factory.CanCreateFor(eventType))
                {
                    var viewModel = ((dynamic)factory).Create((dynamic)uboraEvent, timeStamp);

                    return viewModel;
                }
            }

            if (_generalFactory.CanCreateFor(eventType))
            {
                return _generalFactory.Create(uboraEvent, timeStamp);
            }

            throw new InvalidOperationException("View model factory not found for ubora event type.");
        }
    }
}
