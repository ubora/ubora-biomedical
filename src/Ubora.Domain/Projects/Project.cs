﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Members;
using Ubora.Domain.Projects.DeviceClassification;

namespace Ubora.Domain.Projects
{
    public class Project : Entity<Project>
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string GmdnTerm { get; private set; }
        [Obsolete]
        public string GmdnDefinition { get; private set; }
        [Obsolete]
        public string GmdnCode { get; private set; }
        public string ClinicalNeedTags { get; private set; }
        public string AreaOfUsageTags { get; private set; }
        public string PotentialTechnologyTags { get; private set; }
        public string DescriptionOfNeed { get; private set; }
        public string DescriptionOfExistingSolutionsAndAnalysis { get; private set; }
        public string ProductFunctionality { get; private set; }
        public string ProductPerformance { get; private set; }
        public string ProductUsability { get; private set; }
        public string ProductSafety { get; private set; }
        public string PatientPopulationStudy { get; private set; }
        public string UserRequirementStudy { get; private set; }
        public string AdditionalInformation { get; private set; }
        public string DeviceClassification { get; private set; }

        [JsonProperty(nameof(Members))]
        private readonly HashSet<ProjectMember> _members = new HashSet<ProjectMember>();
        [JsonIgnore]
        public IReadOnlyCollection<ProjectMember> Members => _members;

        private void Apply(ProjectCreatedEvent e)
        {
            Id = e.Id;
            Title = e.Title;
            AreaOfUsageTags = e.AreaOfUsage;
            GmdnCode = e.GmdnCode;
            ClinicalNeedTags = e.ClinicalNeed;
            GmdnDefinition = e.GmdnDefinition;
            GmdnTerm = e.GmdnTerm;
            PotentialTechnologyTags = e.PotentialTechnology;

            var userId = e.InitiatedBy.UserId;
            var leader = new ProjectLeader(userId);

            _members.Add(leader);
        }

        private void Apply(ProjectUpdatedEvent e)
        {
            Title = e.Title;
            ClinicalNeedTags = e.ClinicalNeedTags;
            AreaOfUsageTags = e.AreaOfUsageTags;
            PotentialTechnologyTags = e.PotentialTechnologyTags;
            DescriptionOfNeed = e.DescriptionOfNeed;
            DescriptionOfExistingSolutionsAndAnalysis = e.DescriptionOfExistingSolutionsAndAnalysis;
            ProductFunctionality = e.ProductFunctionality;
            ProductPerformance = e.ProductPerformance;
            ProductUsability = e.ProductUsability;
            ProductSafety = e.ProductSafety;
            PatientPopulationStudy = e.PatientPopulationStudy;
            UserRequirementStudy = e.UserRequirementStudy;
            AdditionalInformation = e.AdditionalInformation;
            GmdnTerm = e.GmdnTerm;
        }

        private void Apply(MemberInvitedToProjectEvent e)
        {
            var member = new ProjectMember(e.UserId);
            _members.Add(member);
        }

        private void Apply(DeviceClassificationSetEvent e)
        {
            DeviceClassification = e.DeviceClassification;
        }

        public override string ToString()
        {
            return $"Project(Id:{Id})";
        }
    }
}
