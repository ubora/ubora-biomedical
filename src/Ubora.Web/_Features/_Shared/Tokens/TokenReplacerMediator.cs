using System.Collections.Generic;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;

namespace Ubora.Web._Features._Shared.Tokens
{
    public class TokenReplacerMediator
    {
        private readonly HtmlEncoder _htmlEncoder;
        private readonly IEnumerable<ITokenReplacer> _replacers;

        public TokenReplacerMediator(HtmlEncoder htmlEncoder, IEnumerable<ITokenReplacer> replacers)
        {
            _replacers = replacers;
            _htmlEncoder = htmlEncoder;
        }

        protected TokenReplacerMediator()
        {
        }

        /// <summary>
        /// Returns encoded HTML-string with Ubora string tokens (e.g. "#project{GUID}") replaced.
        /// </summary>
        public virtual IHtmlContent EncodeAndReplaceAllTokens(string text)
        {
            var encoded = _htmlEncoder.Encode(text);

            foreach (var replacer in _replacers)
            {
                encoded = replacer.ReplaceTokens(encoded);
            }

            return new HtmlString(encoded);
        }
    }
}