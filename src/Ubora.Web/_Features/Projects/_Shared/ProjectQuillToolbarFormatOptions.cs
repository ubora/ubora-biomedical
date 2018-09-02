using Ubora.Web._Features._Shared;

namespace Ubora.Web._Features.Projects._Shared
{
    public class ProjectQuillToolbarFormatOptions : QuillToolbarFormatOptions
    {
        public ProjectQuillToolbarFormatOptions()
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
