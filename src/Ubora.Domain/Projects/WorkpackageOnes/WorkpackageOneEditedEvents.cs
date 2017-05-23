using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.WorkpackageOnes
{
    public abstract class WorkpackageOneEditedEvent : UboraEvent
    {
        protected WorkpackageOneEditedEvent(UserInfo initiatedBy) : base(initiatedBy)
        {
        }

        public string Value { get; set; }
    }

    public class DescriptionOfNeedEdited : WorkpackageOneEditedEvent
    {
        public DescriptionOfNeedEdited(UserInfo initiatedBy) : base(initiatedBy)
        {
        }

        public override string GetDescription()
        {
            return "Description of need was edited.";
        }
    }

    public class DescriptionOfExistingSolutionsAndAnalysisEditedEvent : WorkpackageOneEditedEvent
    {
        public DescriptionOfExistingSolutionsAndAnalysisEditedEvent(UserInfo initiatedBy) : base(initiatedBy)
        {
        }

        public override string GetDescription()
        {
            throw new System.NotImplementedException();
        }
    }

    public class ProductFunctionalityEditedEvent : WorkpackageOneEditedEvent
    {
        public ProductFunctionalityEditedEvent(UserInfo initiatedBy) : base(initiatedBy)
        {
        }

        public override string GetDescription()
        {
            throw new System.NotImplementedException();
        }
    }

    public class ProductPerformanceEditedEvent : WorkpackageOneEditedEvent
    {
        public ProductPerformanceEditedEvent(UserInfo initiatedBy) : base(initiatedBy)
        {
        }

        public override string GetDescription()
        {
            throw new System.NotImplementedException();
        }
    }

    public class ProductUsabilityEditedEvent : WorkpackageOneEditedEvent
    {
        public ProductUsabilityEditedEvent(UserInfo initiatedBy) : base(initiatedBy)
        {
        }

        public override string GetDescription()
        {
            throw new System.NotImplementedException();
        }
    }

    public class ProductSafetyEditedEvent : WorkpackageOneEditedEvent
    {
        public ProductSafetyEditedEvent(UserInfo initiatedBy) : base(initiatedBy)
        {
        }

        public override string GetDescription()
        {
            throw new System.NotImplementedException();
        }
    }

    public class PatientPopulationStudyEditedEvent : WorkpackageOneEditedEvent
    {
        public PatientPopulationStudyEditedEvent(UserInfo initiatedBy) : base(initiatedBy)
        {
        }

        public override string GetDescription()
        {
            throw new System.NotImplementedException();
        }
    }

    public class UserRequirementStudyEditedEvent : WorkpackageOneEditedEvent
    {
        public UserRequirementStudyEditedEvent(UserInfo initiatedBy) : base(initiatedBy)
        {
        }

        public override string GetDescription()
        {
            throw new System.NotImplementedException();
        }
    }

    public class AdditionalInformationEditedEvent : WorkpackageOneEditedEvent
    {
        public AdditionalInformationEditedEvent(UserInfo initiatedBy) : base(initiatedBy)
        {
        }

        public override string GetDescription()
        {
            throw new System.NotImplementedException();
        }
    }
}