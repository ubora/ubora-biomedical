using System;
using System.Collections.Generic;

namespace Ubora.Web._Features.Projects.Workpackages.Reviews
{
    public class WorkpackageReviewListViewModel
    {
        public IEnumerable<WorkpackageReviewViewModel> Reviews { get; set; }
        public string SubmitForReviewUrl { get; set; }
        public string ReviewDecisionUrl { get; set; }
        public Visibility SubmitForReviewButton { get; set; }

    }

    public class Visibility
    {
        public bool IsHiddenWithMessage => !string.IsNullOrWhiteSpace(HideReasonMessage);
        public bool IsHiddenCompletely { get; private set; }
        public string HideReasonMessage { get; private set; }
        public bool IsVisible => !(IsHiddenWithMessage || IsHiddenCompletely);

        protected Visibility()
        {
        }

        public static Visibility Visible()
        {
            return new Visibility();
        }

        public static Visibility CompletelyHidden()
        {
            return new Visibility
            {
                IsHiddenCompletely = true
            };
        }

        public static Visibility HiddenWithMessage(string hideReason)
        {
            if (string.IsNullOrWhiteSpace(hideReason))
            {
                throw new ArgumentException("", nameof(hideReason));
            }
            return new Visibility
            {
                HideReasonMessage = hideReason
            };
        }
    }
}