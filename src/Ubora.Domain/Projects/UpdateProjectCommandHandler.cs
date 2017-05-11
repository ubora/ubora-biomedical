using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects
{
    internal class UpdateProjectCommandHandler : CommandHandler<UpdateProjectCommand>
    {
        public UpdateProjectCommandHandler(IDocumentSession documentSession) : base(documentSession)
        {
        }

        public override ICommandResult Handle(UpdateProjectCommand cmd)
        {
            var project = DocumentSession.Load<Project>(cmd.Id);

            var @event = new ProjectUpdatedEvent(cmd.UserInfo)
            {
                Id = cmd.Id,
                Title = cmd.Title,
                ClinicalNeedTags = cmd.ClinicalNeedTags,
                AreaOfUsageTags = cmd.AreaOfUsageTags,
                PotentialTechnologyTags = cmd.PotentialTechnologyTags,
                DescriptionOfNeed = cmd.DescriptionOfNeed,
                DescriptionOfExistingSolutionsAndAnalysis = cmd.DescriptionOfExistingSolutionsAndAnalysis,
                ProductFunctionality = cmd.ProductFunctionality,
                ProductPerformance = cmd.ProductPerformance,
                ProductUsability = cmd.ProductUsability,
                ProductSafety = cmd.ProductSafety,
                PatientPopulationStudy = cmd.PatientPopulationStudy,
                UserRequirementStudy = cmd.UserRequirementStudy,
                AdditionalInformation = cmd.AdditionalInformation,
                GmdnTerm = cmd.GmdnTerm
            };

            DocumentSession.Events.Append(project.Id, @event);
            DocumentSession.SaveChanges();

            return new CommandResult(true);
        }
    }
}