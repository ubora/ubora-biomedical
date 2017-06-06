using System;
using Newtonsoft.Json;

namespace Ubora.Domain.Projects.WorkpackageOnes
{
    // TODO(Kaspar Kallas): private Apply or immutability
    public class WorkpackageOneStep
    {
        public Guid Id { get; private set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        [JsonConstructor]
        protected WorkpackageOneStep()
        {
        }

        public WorkpackageOneStep(string title, string description)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
        }
    }
}