namespace Ubora.Domain.Resources
{
    public class ResourceContent
    {
        public ResourceContent(string title, QuillDelta body)
        {
            Title = title;
            Body = body;
        }
        
        public string Title { get; }
        public QuillDelta Body { get; }
    }
}