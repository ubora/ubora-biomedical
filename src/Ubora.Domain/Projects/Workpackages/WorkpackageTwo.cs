using System;
using System.Linq;
using Ubora.Domain.Projects.Workpackages.Events;
using Ubora.Domain.Projects.Workpackages.Specifications;

namespace Ubora.Domain.Projects.Workpackages
{
    public class WorkpackageTwo : Workpackage<WorkpackageTwo>
    {
        public bool HasWp1BeenReopened { get; private set; }

        private void Apply(WorkpackageOneReviewAcceptedEvent e)
        {
            if (HasWp1BeenReopened)
            {
                HasWp1BeenReopened = false;
                return;
            }

            if (_steps.Any())
            {
                throw new InvalidOperationException("WP2 has already been opened.");
            }

            ProjectId = e.ProjectId;
            Title = "Conceptual design";
            _steps.Add(new WorkpackageStep("PhysicalPrinciples", "Physical principles"));
            _steps.Add(new WorkpackageStep("ConceptDescription", "Concept description"));
        }

        private void Apply(WorkpackageOneReopenedAfterAcceptanceByReviewEvent e)
        {
            if (HasWp1BeenReopened)
            {
                throw new InvalidOperationException("Already reopened.");
            }
            HasWp1BeenReopened = true;
        }

        private void Apply(WorkpackageTwoStepEdited e)
        {
            var step = _steps.Single(x => x.Id == e.StepId);

            step.Title = e.Title;
            step.Content = e.NewValue;
        }

        private void Apply(WorkpackageTwoSubmittedForReviewEvent e)
        {
            var canApply = this.DoesSatisfy(new CanSubmitWorkpackageReview<WorkpackageTwo>());
            if (!canApply)
            {
                throw new InvalidOperationException();
            }

            var newReview = WorkpackageReview.Create(e.ReviewId, e.SubmittedAt);
            _reviews.Add(newReview);
        }

        private void Apply(WorkpackageTwoReviewAcceptedEvent e)
        {
            var canApply = this.DoesSatisfy(new CanWorkpackageBeAcceptedByReview<WorkpackageTwo>());
            if (!canApply)
            {
                throw new InvalidOperationException();
            }

            var oldReview = GetSingleInProcessReview();
            var acceptedReview = oldReview.ToAccepted(e.ConcludingComment, e.AcceptedAt);

            _reviews.Remove(oldReview);
            _reviews.Add(acceptedReview);
        }

        private void Apply(WorkpackageTwoReviewRejectedEvent e)
        {
            var canApply = this.DoesSatisfy(new CanRejectWorkpackageReview<WorkpackageTwo>());
            if (!canApply)
            {
                throw new InvalidOperationException();
            }

            var oldReview = GetSingleInProcessReview();
            var acceptedReview = oldReview.ToRejected(e.ConcludingComment, e.RejectedAt);

            _reviews.Remove(oldReview);
            _reviews.Add(acceptedReview);
        }
    }
}