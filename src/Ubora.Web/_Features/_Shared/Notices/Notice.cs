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
    }
}