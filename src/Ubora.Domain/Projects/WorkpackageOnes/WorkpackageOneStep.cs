using System;
using Newtonsoft.Json;

namespace Ubora.Domain.Projects.WorkpackageOnes
{
    // TODO(Kaspar Kallas): private Apply or immutability
    public class WorkpackageOneStep
    {
        // Virtual for testing
        public virtual Guid Id { get; private set; }

        public string Title { get; set; }

        public string Value { get; set; }

        [JsonConstructor]
        protected WorkpackageOneStep()
        {
        }

        public WorkpackageOneStep(string title, string value)
        {
            Id = Guid.NewGuid();
            Title = title;
            Value = value;
        }
    }
}