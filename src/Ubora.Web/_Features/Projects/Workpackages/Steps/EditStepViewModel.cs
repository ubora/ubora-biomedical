using Ubora.Web._Features._Shared;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    public class EditStepViewModel : EditStepPostModel
    {
        public string Title { get; set; }
        public string EditStepUrl { get; set; }
        public string ReadStepUrl { get; set; }
        public string Description { get; set; }
    }

    public class ReadStepViewModel : EditStepViewModel
    {
        public string ContentHtml { get; set; }
        public UiElementVisibility EditButton { get; set; }
    }
}