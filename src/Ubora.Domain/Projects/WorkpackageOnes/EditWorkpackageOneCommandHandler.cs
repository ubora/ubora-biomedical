using Marten;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects.WorkpackageOnes
{
    internal class EditWorkpackageOneCommandHandler : CommandHandler,
        ICommandHandler<EditDescriptionOfNeedCommand>,
        ICommandHandler<EditDescriptionOfExistingSolutionsAndAnalysisCommand>,
        ICommandHandler<EditProductFunctionalityCommand>,
        ICommandHandler<EditProductPerformanceCommand>,
        ICommandHandler<EditProductUsabilityCommand>,
        ICommandHandler<EditProductSafetyCommand>,
        ICommandHandler<EditPatientPopulationStudyCommand>,
        ICommandHandler<EditUserRequirementStudyCommand>,
        ICommandHandler<EditAdditionalInformationCommand>
    {
        public EditWorkpackageOneCommandHandler(IDocumentSession documentSession) : base(documentSession)
        {
        }

        public ICommandResult Handle(EditDescriptionOfNeedCommand cmd)
        {
            return HandleInner(cmd);
        }

        private ICommandResult HandleInner(EditDescriptionOfNeedCommand cmd)
        {
            var @event = new DescriptionOfNeedEdited(cmd.Actor)
            {
                Value = cmd.NewValue
            };

            DocumentSession.Events.Append(cmd.ProjectId, @event);
            DocumentSession.SaveChanges();

            return new CommandResult();
        }

        public ICommandResult Handle(EditDescriptionOfExistingSolutionsAndAnalysisCommand cmd)
        {
            var @event = new DescriptionOfExistingSolutionsAndAnalysisEditedEvent(cmd.Actor)
            {
                Value = cmd.NewValue
            };

            DocumentSession.Events.Append(cmd.ProjectId, @event);
            DocumentSession.SaveChanges();

            return new CommandResult();
        }

        public ICommandResult Handle(EditProductFunctionalityCommand cmd)
        {
            var @event = new ProductFunctionalityEditedEvent(cmd.Actor)
            {
                Value = cmd.NewValue
            };

            DocumentSession.Events.Append(cmd.ProjectId, @event);
            DocumentSession.SaveChanges();

            return new CommandResult();
        }

        public ICommandResult Handle(EditProductPerformanceCommand cmd)
        {
            var @event = new ProductPerformanceEditedEvent(cmd.Actor)
            {
                Value = cmd.NewValue
            };

            DocumentSession.Events.Append(cmd.ProjectId, @event);
            DocumentSession.SaveChanges();

            return new CommandResult();
        }

        public ICommandResult Handle(EditProductUsabilityCommand cmd)
        {
            var @event = new ProductUsabilityEditedEvent(cmd.Actor)
            {
                Value = cmd.NewValue
            };

            DocumentSession.Events.Append(cmd.ProjectId, @event);
            DocumentSession.SaveChanges();

            return new CommandResult();
        }

        public ICommandResult Handle(EditProductSafetyCommand cmd)
        {
            var @event = new ProductSafetyEditedEvent(cmd.Actor)
            {
                Value = cmd.NewValue
            };

            DocumentSession.Events.Append(cmd.ProjectId, @event);
            DocumentSession.SaveChanges();

            return new CommandResult();
        }

        public ICommandResult Handle(EditPatientPopulationStudyCommand cmd)
        {
            var @event = new PatientPopulationStudyEditedEvent(cmd.Actor)
            {
                Value = cmd.NewValue
            };

            DocumentSession.Events.Append(cmd.ProjectId, @event);
            DocumentSession.SaveChanges();

            return new CommandResult();
        }

        public ICommandResult Handle(EditUserRequirementStudyCommand cmd)
        {
            var @event = new UserRequirementStudyEditedEvent(cmd.Actor)
            {
                Value = cmd.NewValue
            };

            DocumentSession.Events.Append(cmd.ProjectId, @event);
            DocumentSession.SaveChanges();

            return new CommandResult();
        }

        public ICommandResult Handle(EditAdditionalInformationCommand cmd)
        {
            var @event = new AdditionalInformationEditedEvent(cmd.Actor)
            {
                Value = cmd.NewValue
            };

            DocumentSession.Events.Append(cmd.ProjectId, @event);
            DocumentSession.SaveChanges();

            return new CommandResult();
        }
    }
}