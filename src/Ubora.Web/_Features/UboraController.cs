using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Web.Infrastructure.Extensions;
using Ubora.Web.Services;

namespace Ubora.Web._Features
{
    public abstract class UboraController : Controller
    {
        protected virtual UserInfo UserInfo => User.GetInfo();

        private ICommandQueryProcessor Processor { get; }

        protected UboraController(ICommandQueryProcessor processor)
        {
            Processor = processor;
        }

        protected void ExecuteUserCommand<T>(T command) where T : IUserCommand
        {
            command.Actor = UserInfo;
            var result = Processor.Execute(command);
            if (result.IsFailure)
            {
                ModelState.AddCommandErrors(result);
            }
        }

        protected T ExecuteQuery<T>(IQuery<T> query)
        {
            return Processor.ExecuteQuery(query);
        }

        protected IEnumerable<T> Find<T>(ISpecification<T> specification = null)
        {
            return Processor.Find(specification);
        }

        protected T FindById<T>(Guid id)
        {
            return Processor.FindById<T>(id);
        }
    }
}
