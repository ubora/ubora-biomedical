namespace Ubora.Domain.Resources
{
    public class ResourceContent
    {
        public ResourceContent(string title, string body)
        {
            Title = title;
            Body = body;
        }
        
        public string Title { get; }
        public string Body { get; }
    }
}