using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Candidates.Events
{
    public class CandidateVoteAddedEvent : ProjectEvent
    {
        public CandidateVoteAddedEvent(UserInfo initiatedBy, Guid projectId, Guid candidateId, int functionality, int perfomance, int usability, int safety) 
            : base(initiatedBy, projectId)
        {
            CandidateId = candidateId;
            Functionality = functionality;
            Perfomance = perfomance;
            Usability = usability;
            Safety = safety;
        }
        public Guid CandidateId { get; private set; }
        public int Functionality { get; private set; }  
        public int Perfomance { get; private set; }  
        public int Usability { get; private set; }  
        public int Safety { get; private set; }  

        public override string GetDescription()
        {
            return $"voted for candidate \"{StringTokens.Candidate(CandidateId)}\"";
        }
    }
}
