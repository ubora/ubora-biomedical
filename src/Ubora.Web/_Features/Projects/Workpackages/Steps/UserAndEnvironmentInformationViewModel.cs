using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Ubora.Domain.Projects.StructuredInformations;
using Ubora.Domain.Projects.StructuredInformations.Commands;
using Ubora.Domain.Projects.StructuredInformations.IntendedUsers;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    public class UserAndEnvironmentInformationViewModel
    {
        [Required(ErrorMessage = "The intended user field is required.")]
        public string IntendedUserTypeKey { get; set; }
        [RequiredIf(nameof(IntendedUserTypeKey), "other")]
        public string IntendedUserIfOther { get; set; }

        [Required]
        public bool IsTrainingRequiredInAdditionToExpectedSkillLevelOfIntentedUser { get; set; }
        [RequiredIf(nameof(IsTrainingRequiredInAdditionToExpectedSkillLevelOfIntentedUser), true)]
        public string IfTrainingIsRequiredPleaseDescribeWhoWillDeliverTrainingAndMaterialsAndTimeRequiredForTraining { get; set; }

        [Required]
        public bool IsAnyMaintenanceOrCalibrationRequiredByUserAtTimeOfUse { get; set; }
        public WhereWillTechnologyBeUsedViewModel WhereWillTechnologyBeUsed { get; set; } = new WhereWillTechnologyBeUsedViewModel();

        public class Factory
        {
            private readonly IMapper _autoMapper;

            public Factory(IMapper autoMapper)
            {
                _autoMapper = autoMapper;
            }

            protected Factory()
            {
            }

            public virtual UserAndEnvironmentInformationViewModel Create(UserAndEnvironmentInformation domainAggregate)
            {
                var model = new UserAndEnvironmentInformationViewModel();

                model.IntendedUserTypeKey = domainAggregate.IntendedUser.Key;
                if (domainAggregate.IntendedUser.GetType() == typeof(Other))
                {
                    model.IntendedUserIfOther = domainAggregate.IntendedUser.ToDisplayName();
                }

                model.IsTrainingRequiredInAdditionToExpectedSkillLevelOfIntentedUser =
                    domainAggregate.IntendedUserTraining.IsTrainingRequiredInAdditionToExpectedSkillLevel;

                if (domainAggregate.IntendedUserTraining.IsTrainingRequiredInAdditionToExpectedSkillLevel)
                {
                    model.IfTrainingIsRequiredPleaseDescribeWhoWillDeliverTrainingAndMaterialsAndTimeRequiredForTraining =
                        domainAggregate.IntendedUserTraining.DescriptionOfWhoWillDeliverTrainingAndMaterialsAndTimeRequired;
                }

                model.IsAnyMaintenanceOrCalibrationRequiredByUserAtTimeOfUse = domainAggregate.IsAnyMaintenanceOrCalibrationRequiredByIntentedUserAtTimeOfUse;
                model.WhereWillTechnologyBeUsed = _autoMapper.Map<WhereWillTechnologyBeUsedViewModel>(domainAggregate.WhereWillTechnologyBeUsed);

                return model;
            }
        }

        public class Mapper
        {
            public virtual EditUserAndEnvironmentInformationCommand MapToCommand(UserAndEnvironmentInformationViewModel model)
            {
                var userAndEnvironmentInformation = new UserAndEnvironmentInformation(
                    intendedUser: MapIntendedUser(model),
                    intendedUserTraining: MapIntendedUserTraining(model),
                    isAnyMaintenanceOrCalibrationRequiredByIntentedUserAtTimeOfUse: model.IsAnyMaintenanceOrCalibrationRequiredByUserAtTimeOfUse,
                    whereWillTechnologyBeUsed: MapWhereWillTechnologyBeUsed(model));

                return new EditUserAndEnvironmentInformationCommand
                {
                    UserAndEnvironmentInformation = userAndEnvironmentInformation
                };
            }

            private IntendedUserTraining MapIntendedUserTraining(UserAndEnvironmentInformationViewModel model)
            {
                if (model.IsTrainingRequiredInAdditionToExpectedSkillLevelOfIntentedUser)
                {
                    return IntendedUserTraining.CreateTrainingRequired(model.IfTrainingIsRequiredPleaseDescribeWhoWillDeliverTrainingAndMaterialsAndTimeRequiredForTraining);
                }
                else
                {
                    return IntendedUserTraining.CreateTrainingNotRequired();
                }
            }

            private WhereWillTechnologyBeUsed MapWhereWillTechnologyBeUsed(UserAndEnvironmentInformationViewModel model)
            {
                return new WhereWillTechnologyBeUsed
                (
                    tertiaryLevel: model.WhereWillTechnologyBeUsed.TertiaryLevel,
                    secondaryLevel: model.WhereWillTechnologyBeUsed.SecondaryLevel,
                    urbanSettings: model.WhereWillTechnologyBeUsed.UrbanSettings,
                    indoors: model.WhereWillTechnologyBeUsed.Indoors,
                    ruralSettings: model.WhereWillTechnologyBeUsed.RuralSettings,
                    outdoors: model.WhereWillTechnologyBeUsed.Outdoors,
                    primaryLevel: model.WhereWillTechnologyBeUsed.PrimaryLevel,
                    atHome: model.WhereWillTechnologyBeUsed.AtHome
                );
            }

            private IntendedUser MapIntendedUser(UserAndEnvironmentInformationViewModel model)
            {
                if (model.IntendedUserTypeKey == null)
                {
                    return new EmptyIntendedUser();
                }

                var isIntendedUserSpecified = IntendedUser.IntendedUserKeyTypeMap
                    .TryGetValue(model.IntendedUserTypeKey, out Type intendedUserType);

                if (isIntendedUserSpecified)
                {
                    if (intendedUserType == typeof(Other))
                    {
                        return new Other(model.IntendedUserIfOther);
                    }
                    else
                    {
                        return (IntendedUser)Activator.CreateInstance(type: intendedUserType);
                    }
                }
                else
                {
                    return null;
                }
            }
        }
    }

    public class WhereWillTechnologyBeUsedViewModel
    {
        public bool RuralSettings { get; set; }
        public bool UrbanSettings { get; set; }
        public bool Outdoors { get; set; }
        public bool Indoors { get; set; }
        public bool AtHome { get; set; }
        public bool PrimaryLevel { get; set; }
        public bool SecondaryLevel { get; set; }
        public bool TertiaryLevel { get; set; }

        public IEnumerable<string> GetResult()
        {
            if (RuralSettings)
            {
                yield return "rural settings";
            }

            if (UrbanSettings)
            {
                yield return "urban settings";
            }

            if (Outdoors)
            {
                yield return "outdoors";
            }

            if (Indoors)
            {
                yield return "indoors";
            }

            if (AtHome)
            {
                yield return "at home";
            }

            if (PrimaryLevel)
            {
                yield return "primary level (health post, health center)";
            }

            if (SecondaryLevel)
            {
                yield return "secondary level (general hospital)";
            }

            if (TertiaryLevel)
            {
                yield return "tertiary level (specialist hospital)";
            }
        }
    }
}
