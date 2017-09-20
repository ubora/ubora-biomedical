using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Web._Features.Projects.History._Base
{
    public interface IEventViewModel<T> : IEventViewModel where T : UboraEvent
    {
    }

    public interface IEventViewModel
    {
        DateTimeOffset Timestamp { get; set; }
        IHtmlContent GetPartialView(IHtmlHelper htmlHelper);
    }
}
