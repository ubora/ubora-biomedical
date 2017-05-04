using System;
using System.Collections.Generic;

namespace Ubora.Domain.Projects
{
    public class Project
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        [Obsolete]
        public string Description { get; set; }

        [Obsolete]
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
        public string ProductPerformance { get; private set; }
        public string ProductUsability { get; private set; }
        public string ProductSafety { get; private set; }
        public string PatientsTargetGroup { get; private set; }
        public string EndusersTargetGroup { get; private set; }
        public string AdditionalInformation { get; private set; }

        private readonly HashSet<ProjectMember> _members = new HashSet<ProjectMember>();
        public IReadOnlyCollection<ProjectMember> Members => _members;

        private void Apply(ProjectCreatedEvent e)
        {
            Id = e.Id;
            Title = e.Title;
            Description = e.Description;
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
            //WorkpackageOne = new WorkpackageOne
            //{
            //    Functionality = e.Functionality,
            //    Performance = e.Performance,
            //    Safety = e.Safety,
            //    Usability = e.Usability
            //};
        }

        public override string ToString()
        {
            return $"Project(Id:{Id})";
        }
    }
}
