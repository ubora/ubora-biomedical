using System;

namespace Ubora.Web._Features._Shared
{
    public class UiElementVisibility
    {
        public bool IsHiddenWithMessage => !string.IsNullOrWhiteSpace(HideReasonMessage);
        public bool IsHiddenCompletely { get; private set; }
        public string HideReasonMessage { get; private set; }
        public bool IsVisibleRequestMentoring { get; private set; }
        public bool IsVisible => !(IsHiddenWithMessage || IsHiddenCompletely || IsVisibleRequestMentoring);

        protected UiElementVisibility()
        {
        }

        public static UiElementVisibility Visible()
        {
            return new UiElementVisibility();
        }

        public static UiElementVisibility HiddenWithMessage(string hideReason)
        {
            if (string.IsNullOrWhiteSpace(hideReason))
            {
                throw new ArgumentException("", nameof(hideReason));
            }
            return new UiElementVisibility
            {
                HideReasonMessage = hideReason
            };
        }

        public static UiElementVisibility HiddenCompletely()
        {
            return new UiElementVisibility
            {
                IsHiddenCompletely = true
            };
        }

        public static UiElementVisibility RequestMentoring()
        {
            return new UiElementVisibility
            {
                IsVisibleRequestMentoring = true
            };
        }
    }
}