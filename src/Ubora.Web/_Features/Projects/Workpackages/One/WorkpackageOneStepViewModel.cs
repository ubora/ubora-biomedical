namespace Ubora.Web._Features.Projects.Workpackages.One
{
    public class WorkpackageOneStepViewModel
    {
        public string Title { get; set; }
        public bool IsSelected { get; set; }

        public string ActionName { get; set; }
        public string ControllerName => nameof(WorkpackageOneStepsController).Replace("Controller", "");
    }
}