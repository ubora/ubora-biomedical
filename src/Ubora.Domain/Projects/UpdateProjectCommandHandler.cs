using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects
{
    public class UpdateProjectCommandHandler : ICommandHandler<UpdateProjectCommand>
    {
        private readonly IDocumentSession _documentSession;

        public UpdateProjectCommandHandler(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public ICommandResult Handle(UpdateProjectCommand command)
        {
            var project = _documentSession.Load<Project>(command.Id);

            var @event = new ProjectUpdatedEvent(command.UserInfo)
            {
                Id = command.Id,
                Title = command.Title,
                ClinicalNeedTags = command.ClinicalNeedTags,
                AreaOfUsageTags = command.AreaOfUsageTags,
                PotentialTechnologyTags = command.PotentialTechnologyTags,
                DescriptionOfNeed = command.DescriptionOfNeed,
                DescriptionOfExistingSolutionsAndAnalysis = command.DescriptionOfExistingSolutionsAndAnalysis,
                ProductFunctionality = command.ProductFunctionality,
                ProductPerformance = command.ProductPerformance,
                ProductUsability = command.ProductUsability,
                ProductSafety = command.ProductSafety,
                PatientPopulationStudy = command.PatientPopulationStudy,
                UserRequirementStudy = command.UserRequirementStudy,
                AdditionalInformation = command.AdditionalInformation,
                GmdnTerm = command.GmdnTerm
            };

            _documentSession.Events.Append(project.Id, @event);
            _documentSession.SaveChanges();

            return new CommandResult(true);
        }
    }
}