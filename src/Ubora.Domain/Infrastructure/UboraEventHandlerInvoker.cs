﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Marten;
using Marten.Events;
using Marten.Services;

namespace Ubora.Domain.Infrastructure
{
    public class UboraEventHandlerInvoker : DocumentSessionListenerBase
    {
        private readonly IComponentContext _serviceLocator;

        public UboraEventHandlerInvoker(IComponentContext serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public override void AfterCommit(IDocumentSession session, IChangeSet commit)
        {
            InvokeUboraEventHandlers(session, commit);
        }

        private void InvokeUboraEventHandlers(IDocumentSession session, IChangeSet commit)
        {
            var commitedEvents = commit.GetEvents();
            
            foreach (var @event in commitedEvents)
            {
                dynamic eventHandlers = ResolveEventHandlers(@event);

                foreach (var eventHandler in eventHandlers)
                {
                    eventHandler.Handle(@event);
                }
            }

            dynamic ResolveEventHandlers(IEvent @event)
            {
                var handlerType = typeof(IUboraEventHandler<>).MakeGenericType(@event.Data.GetType());
                var enumerableHandlerType = typeof(IEnumerable<>).MakeGenericType(handlerType);

                var eventHandlers = (dynamic)_serviceLocator.Resolve(enumerableHandlerType);
                return eventHandlers;
            }
        }
    }
}
