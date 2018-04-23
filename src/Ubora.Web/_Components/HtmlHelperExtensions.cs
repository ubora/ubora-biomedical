using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ubora.Web._Components
{
    public static class HtmlHelperExtensions
    {
        public static HtmlString TimeAgoHistorySpan(this IHtmlHelper helper, DateTimeOffset dateTimeOffset)
        {
            var titleAttribute = dateTimeOffset.ToString("o");
            var textToDisplayAfterTimeAgoOffsetIsOver = dateTimeOffset.ToString("d");
            
            return new HtmlString($"<span class=\"history-timestamp timeago\" title=\"{titleAttribute}\">{textToDisplayAfterTimeAgoOffsetIsOver}</span>");
        }
    }
}