using System;
using System.Collections.Generic;
using System.Linq;
using Marten.Schema;
using Newtonsoft.Json;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.WorkpackageTwos;

namespace Ubora.Domain.Projects.WorkpackageOnes
{
    public abstract class Workpackage<TDerived> : Entity<TDerived> where TDerived : Entity<TDerived>
    {
        [Identity]
        public Guid ProjectId { get; protected set; }

        public string Title { get; protected set; }

        [JsonProperty(nameof(Steps))]
        protected readonly HashSet<WorkpackageOneStep> _steps = new HashSet<WorkpackageOneStep>();
        [JsonIgnore]
        public IReadOnlyCollection<WorkpackageOneStep> Steps => _steps;

        [JsonProperty(nameof(Reviews))]
        protected readonly HashSet<WorkpackageReview> _reviews = new HashSet<WorkpackageReview>();
        [JsonIgnore]
        public IReadOnlyCollection<WorkpackageReview> Reviews => _reviews;

        public bool IsLocked { get; protected set; }

        // Virtual for testing
        public virtual WorkpackageOneStep GetSingleStep(Guid stepId)
        {
            return _steps.Single(step => step.Id == stepId);
        }
    }

    public class WorkpackageOne : Workpackage<WorkpackageOne>
    {
        private void Apply(WorkpackageOneOpenedEvent e)
        {
            ProjectId = e.ProjectId;

            Title = "Design and prototyping";

            _steps.Add(new WorkpackageOneStep("Description Of Need", Placeholders.DescriptionOfNeed));
            _steps.Add(new WorkpackageOneStep("Description Of Existing Solutions And Analysis", Placeholders.DescriptionOfExistingSolutionsAndAnalysis));
            _steps.Add(new WorkpackageOneStep("Product Functionality", Placeholders.ProductFunctionality));
            _steps.Add(new WorkpackageOneStep("Product Performance", Placeholders.ProductPerformance));
            _steps.Add(new WorkpackageOneStep("Product Usability", Placeholders.ProductUsability));
            _steps.Add(new WorkpackageOneStep("Product Safety", Placeholders.ProductSafety));
            _steps.Add(new WorkpackageOneStep("Patient Population Study", Placeholders.PatientPopulationStudy));
            _steps.Add(new WorkpackageOneStep("User Requirement Study", Placeholders.UserRequirementStudy));
            _steps.Add(new WorkpackageOneStep("Additional Information", Placeholders.AdditionalInformation));
        }

        private void Apply(WorkpackageOneStepEditedEvent e)
        {
            var canApply = this.DoesSatisfy(new CanBeEdited(e.StepId));
            if (!canApply)
            {
                throw new InvalidOperationException();
            }

            var step = Steps.Single(x => x.Id == e.StepId);

            step.Title = e.Title;
            step.Content = e.NewValue;
        }

        private void Apply(WorkpackageOneSubmittedForReviewEvent e)
        {
            var canApply = this.DoesSatisfy(new CanBeSubmittedForReview<WorkpackageOne>());
            if (!canApply)
            {
                throw new InvalidOperationException();
            }

            var newReview = new WorkpackageReview(
                id: Guid.NewGuid(),
                status: WorkpackageReviewStatus.InReview);
            _reviews.Add(newReview);

            // todo: state pattern
            IsLocked = true;
        }

        private void Apply(WorkpackageOneAcceptedByReviewEvent e)
        {
            var canApply = this.DoesSatisfy(new CanWorkpackageBeAcceptedByReview<WorkpackageOne>());
            if (!canApply)
            {
                throw new InvalidOperationException();
            }

            var oldReview = _reviews.Single(review => review.Id == e.ReviewId);
            var acceptedReview = oldReview.ToStatus(WorkpackageReviewStatus.Accepted);

            _reviews.Remove(oldReview);
            _reviews.Add(acceptedReview);
        }

        private void Apply(WorkpackageOneRejectedByReviewEvent e)
        {
            var canApply = this.DoesSatisfy(new CanWorkpackageBeRejectedByReview<WorkpackageOne>());
            if (!canApply)
            {
                throw new InvalidOperationException();
            }

            var oldReview = _reviews.Single(review => review.Id == e.ReviewId);
            var acceptedReview = oldReview.ToStatus(WorkpackageReviewStatus.Rejected);

            _reviews.Remove(oldReview);
            _reviews.Add(acceptedReview);

            // todo: state pattern
            IsLocked = false;
        }
    }
}