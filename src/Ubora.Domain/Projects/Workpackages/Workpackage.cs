using System;
using System.Collections.Generic;
using System.Linq;
using Marten.Schema;
using Newtonsoft.Json;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Workpackages.Specifications;

namespace Ubora.Domain.Projects.Workpackages
{
    public abstract class Workpackage<TDerived> : Entity<TDerived> where TDerived : Workpackage<TDerived>
    {
        [Identity]
        public Guid ProjectId { get; protected set; }

        public string Title { get; protected set; }

        [JsonProperty(nameof(Steps))]
        protected readonly HashSet<WorkpackageStep> _steps = new HashSet<WorkpackageStep>();
        [JsonIgnore]
        public IReadOnlyCollection<WorkpackageStep> Steps => _steps;

        [JsonProperty(nameof(Reviews))]
        protected readonly HashSet<WorkpackageReview> _reviews = new HashSet<WorkpackageReview>();

        [JsonIgnore]
        // Virtual for testing
        public virtual IReadOnlyCollection<WorkpackageReview> Reviews => _reviews;

        public bool HasReviewInProcess => this.DoesSatisfy(new HasReviewInStatus<TDerived>(WorkpackageReviewStatus.InProcess));
        public bool HasBeenAccepted => this.DoesSatisfy(new HasReviewInStatus<TDerived>(WorkpackageReviewStatus.Accepted));

        // Virtual for testing
        public virtual WorkpackageStep GetSingleStep(string stepKey)
        {
            return _steps.Single(step => step.Id == stepKey);
        }

        public virtual WorkpackageReview GetSingleActiveReview()
        {
            return _reviews.Single(x => x.Status == WorkpackageReviewStatus.InProcess);
        }
    }
}