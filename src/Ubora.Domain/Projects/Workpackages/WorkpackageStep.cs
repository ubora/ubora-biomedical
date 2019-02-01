using System;
using Newtonsoft.Json;

namespace Ubora.Domain.Projects.Workpackages
{
    // TODO(Kaspar Kallas): private Apply or immutability
    public class WorkpackageStep
    {
        public string Id { get; private set; }

        public string Title { get; set; }

        [JsonIgnore]
        public string Description => Placeholders.ResourceManager.GetString(Id);

        [Obsolete]
        public string Content { get; internal set; }

        public QuillDelta ContentV2 { get; internal set; }

        [JsonConstructor]
        protected WorkpackageStep()
        {
        }

        public WorkpackageStep(string id, string title)
        {
            Id = id;
            Title = title;
            ContentV2 = new QuillDelta();
        }
    }
}