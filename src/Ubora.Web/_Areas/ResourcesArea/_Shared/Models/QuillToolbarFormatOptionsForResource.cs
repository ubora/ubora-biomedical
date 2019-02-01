using Ubora.Web._Features._Shared;

namespace Ubora.Web._Areas.ResourcesArea._Shared.Models
{
    public class QuillToolbarFormatOptionsForResource : QuillToolbarFormatOptions
    {
        public QuillToolbarFormatOptionsForResource()
        {
            TextDecorations = true;
            TextColorAndBackground = true;
            TextSubAndSuperScript = true;
            Headers = true;
            Blockquote = true;
            CodeBlock = true;
            Lists = true;
            TextDirection = true;
            TextAlignment = true;
            Hyperlinks = true;
            Images = true;
            Videos = true;
            CleanFormats = true;
        }
    }
}
