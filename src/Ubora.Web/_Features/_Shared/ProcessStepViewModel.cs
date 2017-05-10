using System;

namespace Ubora.Web._Features._Shared
{
    public class ProcessStepViewModel
    {
        public Guid ProjectId { get; set; }
        public StepStatus StepOneStatus { get; set; }
        public StepStatus StepTwoStatus { get; set; }
        public string StepOneStatusClass => GetClass(StepOneStatus);
        public string StepTwoStatusClass => GetClass(StepTwoStatus);

        private string GetClass(StepStatus stepOneStatus)
        {
            switch (stepOneStatus)
            {
                case StepStatus.Inactive:
                    return "";
                case StepStatus.Completed:
                    return "process-step-completed";
                case StepStatus.Active:
                    return "process-step-active";
                case StepStatus.Unfinished:
                    return "process-step-unfinished";
                default:
                    throw new ArgumentOutOfRangeException(nameof(stepOneStatus));
            }
        }
    }

    public enum StepStatus
    {
        Inactive,
        Completed,
        Active,
        Unfinished
    }
}
