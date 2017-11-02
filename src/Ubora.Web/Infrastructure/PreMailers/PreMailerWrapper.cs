using AngleSharp.Dom.Html;
using PreMailer.Net;

namespace Ubora.Web.Infrastructure.PreMailers
{
    /// <summary>
    /// Wrapper class for testing purposes.
    /// </summary>
    public class PreMailerWrapper
    {
        private readonly PreMailer.Net.PreMailer _preMailer;

        public PreMailerWrapper(PreMailer.Net.PreMailer preMailer)
        {
            _preMailer = preMailer;
        }

        public PreMailerWrapper()
        {
            
        }

        public virtual InlineResult MoveCssInline(bool removeStyleElements = false, string ignoreElements = null, string css = null, bool stripIdAndClassAttributes = false, bool removeComments = false)
        {
            return _preMailer.MoveCssInline(removeStyleElements, ignoreElements, css, stripIdAndClassAttributes, removeComments);
        }

        public PreMailer.Net.PreMailer AddAnalyticsTags(string source, string medium, string campaign, string content, string domain = null)
        {
            return _preMailer.AddAnalyticsTags(source, medium, campaign, content, domain);
        }

        public void Dispose()
        {
            _preMailer.Dispose();
        }

        public IHtmlDocument Document => _preMailer.Document;
    }
}