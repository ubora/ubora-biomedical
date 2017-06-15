using System;
using System.Collections.Generic;
using System.Linq;
using Marten.Schema;
using Newtonsoft.Json;
using Ubora.Domain.Infrastructure;

namespace Ubora.Domain.Projects.Workpackages
{
    public abstract class Workpackage<TDerived> : Entity<TDerived> where TDerived : Entity<TDerived>
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
        public IReadOnlyCollection<WorkpackageReview> Reviews => _reviews;

        // TODO: Get rid of these? Calculate somehow
        public bool IsVisible { get; protected set; }

        // Virtual for testing
        public virtual WorkpackageStep GetSingleStep(Guid stepId)
        {
            return _steps.Single(step => step.Id == stepId);
        }

        public virtual WorkpackageReview GetSingleActiveReview()
        {
            return _reviews.Single(x => x.Status == WorkpackageReviewStatus.InReview);
        }
    }
}