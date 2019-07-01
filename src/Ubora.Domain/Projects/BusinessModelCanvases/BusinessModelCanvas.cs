using System;
using Ubora.Domain.Projects.BusinessModelCanvases.Events;
using Ubora.Domain.Projects.Workpackages.Events;

namespace Ubora.Domain.Projects.BusinessModelCanvases
{
    public class BusinessModelCanvas : IProjectEntity
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }

        public QuillDelta ValueProposalDescription { get; private set; }
        public QuillDelta GrowthStrategyDescription { get; private set; }
        public QuillDelta KeyResourcesAndPartnersDescription { get; private set; }
        public QuillDelta PotentialClientsAndUsersAndChannelsDescription { get; private set; }
        public QuillDelta RelevantDocumentationForProductionAndUseDescription { get; private set; }
        public QuillDelta AnalysisOfCostsAndProductionAndSupplyChainAndServicesToClientsDescription { get; private set; }

        private void Apply(WorkpackageFiveOpenedEvent e)
        {
            ProjectId = e.ProjectId;
            ValueProposalDescription = new QuillDelta();
            KeyResourcesAndPartnersDescription = new QuillDelta();
            AnalysisOfCostsAndProductionAndSupplyChainAndServicesToClientsDescription = new QuillDelta();
            GrowthStrategyDescription = new QuillDelta();
            PotentialClientsAndUsersAndChannelsDescription = new QuillDelta();
            RelevantDocumentationForProductionAndUseDescription = new QuillDelta();
        }

        private void Apply(ValueProposalDescriptionEditedEvent e)
        {
            ValueProposalDescription = e.Value ?? throw new InvalidOperationException("NULL not allowed.");
        }

        private void Apply(GrowthStrategyDescriptionEditedEvent e)
        {
            GrowthStrategyDescription = e.Value ?? throw new InvalidOperationException("NULL not allowed.");
        }

        private void Apply(KeyResourcesAndPartnersDescriptionEditedEvent e)
        {
            KeyResourcesAndPartnersDescription = e.Value ?? throw new InvalidOperationException("NULL not allowed.");
        }

        private void Apply(PotentialClientsAndUsersAndChannelsDescriptionEditedEvent e)
        {
            PotentialClientsAndUsersAndChannelsDescription = e.Value ?? throw new InvalidOperationException("NULL not allowed.");
        }

        private void Apply(RelevantDocumentationForProductionAndUseDescriptionEditedEvent e)
        {
            RelevantDocumentationForProductionAndUseDescription = e.Value ?? throw new InvalidOperationException("NULL not allowed.");
        }

        private void Apply(AnalysisOfCostsAndProductionAndSupplyChainAndServicesToClientsDescriptionEditedEvent e)
        {
            AnalysisOfCostsAndProductionAndSupplyChainAndServicesToClientsDescription = e.Value ?? throw new InvalidOperationException("NULL not allowed.");
        }
    }
}
