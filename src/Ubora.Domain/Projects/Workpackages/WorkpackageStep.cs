using Newtonsoft.Json;

namespace Ubora.Domain.Projects.Workpackages
{
    // TODO(Kaspar Kallas): private Apply or immutability
    public class WorkpackageStep
    {
        public string Id { get; private set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        [JsonConstructor]
        protected WorkpackageStep()
        {
        }

        public WorkpackageStep(string id, string title, string description)
        {
            Id = id;
            Title = title;
            Description = description;
        }
    }
}