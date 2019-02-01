using System;
using System.Linq;
using Ubora.Domain.Projects.Workpackages.Events;

namespace Ubora.Domain.Projects.Workpackages
{
    public class WorkpackageThree : Workpackage<WorkpackageThree>
    {
        public bool HasBeenOpened { get; private set ; }
        
        private void Apply(WorkpackageThreeOpenedEvent e)
        {
            if (_steps.Any())
            {
                throw new InvalidOperationException("Already opened.");
            }

            ProjectId = e.ProjectId;
            Title = "Design and prototyping";
            HasBeenOpened = true;

            _steps.Add(new WorkpackageStep("GeneralProductDescription_Hardware_CommercialParts", "Commercial parts"));
            _steps.Add(new WorkpackageStep("GeneralProductDescription_Hardware_PurposelyDesignedParts", "Purposely designed parts"));
            _steps.Add(new WorkpackageStep("GeneralProductDescription_Hardware_PrototypesAndFunctionalTrials", "Prototypes and functional trials"));

            _steps.Add(new WorkpackageStep("GeneralProductDescription_ElectronicAndFirmware_CommercialParts", "Commercial parts"));
            _steps.Add(new WorkpackageStep("GeneralProductDescription_ElectronicAndFirmware_PurposelyDesignedParts", "Purposely designed parts"));
            _steps.Add(new WorkpackageStep("GeneralProductDescription_ElectronicAndFirmware_PrototypesAndFunctionalTrials", "Prototypes and functional trials"));

            _steps.Add(new WorkpackageStep("GeneralProductDescription_Software_ExistingSolutions", "Existing solutions (open source)"));
            _steps.Add(new WorkpackageStep("GeneralProductDescription_Software_PurposelyDesignedParts", "Purposely designed parts"));
            _steps.Add(new WorkpackageStep("GeneralProductDescription_Software_PrototypesAndFunctionalTrials", "Prototypes and functional trials"));

            _steps.Add(new WorkpackageStep("GeneralProductDescription_SystemIntegration_PrototypesAndFunctionalTrials", "Prototypes and functional trials"));

            _steps.Add(new WorkpackageStep("DesignForIsoTestingCompliance", "Design for ISO testing compliance"));
            _steps.Add(new WorkpackageStep("InstructionsForFabricationOfPrototypes", "Instructions for fabrication of prototypes"));
        }

        [Obsolete]
        private void Apply(WorkpackageThreeStepEdited e)
        {
            var step = GetSingleStep(e.StepId);

            step.Title = e.Title;
            step.Content = e.NewValue;
        }

        private void Apply(WorkpackageThreeStepEditedEventV2 e)
        {
            var step = _steps.Single(x => x.Id == e.StepId);

            step.Title = e.Title;
            step.ContentV2 = e.NewValue;
        }
    }
}