using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects.WorkpackageOnes
{
    public abstract class EditWorkpackageOneCommand : UserProjectCommand
    {
        public string NewValue { get; set; }
    }

    public class EditDescriptionOfNeedCommand : EditWorkpackageOneCommand
    {
    }

    public class EditDescriptionOfExistingSolutionsAndAnalysisCommand : EditWorkpackageOneCommand
    {
    }

    public class EditProductFunctionalityCommand : EditWorkpackageOneCommand
    {
    }

    public class EditProductPerformanceCommand : EditWorkpackageOneCommand
    {
    }

    public class EditProductUsabilityCommand : EditWorkpackageOneCommand
    {
    }

    public class EditProductSafetyCommand : EditWorkpackageOneCommand
    {
    }

    public class EditPatientPopulationStudyCommand : EditWorkpackageOneCommand
    {
    }

    public class EditUserRequirementStudyCommand : EditWorkpackageOneCommand
    {
    }

    public class EditAdditionalInformationCommand : EditWorkpackageOneCommand
    {
    }
}
