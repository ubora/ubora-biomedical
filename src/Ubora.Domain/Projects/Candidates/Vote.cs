using System;
using Newtonsoft.Json;

namespace Ubora.Domain.Projects.Candidates
{
    public class Vote
    {
        public Vote(Guid userId, int functionality, int performance, int usability, int safety)
        {
            UserId = userId;
            Functionality = functionality;
            Performance = performance;
            Usability = usability;
            Safety = safety;
        }

        public Guid UserId { get; private set; }
        public int Functionality { get; private set; }
        public int Performance { get; private set; }
        public int Usability { get; private set; }
        public int Safety { get; private set; }

        [JsonIgnore]
        public decimal Score => Functionality + Performance + Usability + Safety;
    }
}
