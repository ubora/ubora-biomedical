using Microsoft.AspNetCore.Html;
using System;
using Ubora.Web._Features.Projects.History._Base;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ubora.Web._Features._Shared.Tokens;
using Ubora.Domain.Infrastructure.Events;
using System.Reflection;

namespace Ubora.Web._Features.Projects.History
{
    public class GeneralEventViewModel : IEventViewModel
    {
        public IHtmlContent Message { get; set; }
        public DateTimeOffset Timestamp { get; set; }

        public IHtmlContent GetPartialView(IHtmlHelper htmlHelper)
        {
            return htmlHelper.Partial("~/_Features/Projects/History/_GeneralEventPartial.cshtml", this);
        }

        public class Factory : IEventViewModelFactory
        {
            private readonly TokenReplacerMediator _tokenReplacerMediator;

            public Factory(TokenReplacerMediator tokenReplacerMediator)
            {
                _tokenReplacerMediator = tokenReplacerMediator;
            }

            public bool CanCreateFor(Type type)
            {
                return typeof(UboraEvent).IsAssignableFrom(type);
            }

            public GeneralEventViewModel Create(UboraEvent generalEvent, DateTimeOffset timestamp)
            {
                return new GeneralEventViewModel
                {
                    Message = _tokenReplacerMediator.EncodeAndReplaceAllTokens(generalEvent.ToString()),
                    Timestamp = timestamp
                };
            }
        }
    }
}
