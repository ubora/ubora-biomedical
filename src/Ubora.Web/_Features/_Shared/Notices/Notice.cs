using System;

namespace Ubora.Web._Features._Shared.Notices
{
    public class Notice
    {
        public Notice(string text, NoticeType type)
        {
            Text = text;
            Type = type;
        }

        public string Text { get; private set; }
        public NoticeType Type { get; private set; }

        public static Notice Success(string text)
        {
            return new Notice(text, NoticeType.Success);
        }

        public static Notice None(string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new ArgumentException("Give me a reason you are not displaying a notice.");
            }
            return new Notice("", NoticeType.None);
        }
    }
}