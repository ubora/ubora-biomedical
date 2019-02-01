using Ubora.Web._Features._Shared;

namespace Ubora.Web._Areas.ClinicalNeedsArea._Shared
{
    public class QuillToolbarFormatOptionsForClinicalNeed : QuillToolbarFormatOptions
    {
        public QuillToolbarFormatOptionsForClinicalNeed()
        {
            TextDecorations = true;
            TextSubAndSuperScript = true;
            Blockquote = true;
            Lists = true;
            Hyperlinks = true;
            Images = true;
            Videos = true;
        }
    }
}
