using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects
{
    // TODO: Jäi poolikuks
    public class UpdateProjectCommand : ICommand
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ClinicalNeedTags { get; set; }
        public string AreaOfUsageTags { get; set; }
        public string PotentialTechnologyTags { get; set; }
        public string DescriptionOfNeed { get; set; }
        public string DescriptionOfExistingSolutionsAndAnalysis { get; set; }
        public string ProductPerformance { get; set; }
        public string ProductUsability { get; set; }
        public string ProductSafety { get; set; }
        public string PatientsTargetGroup { get; set; }
        public string EndusersTargetGroup { get; set; }
        public string AdditionalInformation { get; set; }
        public UserInfo UserInfo { get; set; }
    }

    public class UpdateProjectCommandHandler : ICommandHandler<UpdateProjectCommand>
    {
        private readonly IDocumentSession _documentSession;

        public UpdateProjectCommandHandler(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public ICommandResult Handle(UpdateProjectCommand command)
        {
            throw new NotImplementedException();
        }
    }

    public class ProjectUpdatedEvent : UboraEvent
    {
        public string Functionality { get; set; }
        public string Performance { get; set; }
        public string Usability { get; set; }
        public string Safety { get; set; }

        public ProjectUpdatedEvent(UserInfo initiatedBy) : base(initiatedBy)
        {
        }

        public override string GetDescription()
        {
            return "Updated project";
        }
    }
}
