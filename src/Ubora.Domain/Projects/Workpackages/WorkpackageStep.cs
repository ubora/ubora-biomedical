using System;
using Newtonsoft.Json;

namespace Ubora.Domain.Projects.Workpackages
{
    // TODO(Kaspar Kallas): private Apply or immutability
    public class WorkpackageStep
    {
        public Guid Id { get; private set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        [JsonConstructor]
        protected WorkpackageStep()
        {
        }

        public WorkpackageStep(string title, string description)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
        }
    }
}