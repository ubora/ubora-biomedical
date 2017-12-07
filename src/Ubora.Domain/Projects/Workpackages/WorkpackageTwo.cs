using System;
using System.Linq;
using Ubora.Domain.Projects.Workpackages.Events;
using Ubora.Domain.Projects.Workpackages.Specifications;

namespace Ubora.Domain.Projects.Workpackages
{
    public class WorkpackageTwo : Workpackage<WorkpackageTwo>
    {
        private void Apply(WorkpackageOneReviewAcceptedEvent e)
        {
            ProjectId = e.ProjectId;

            Title = "Conceptual design";

            _steps.Add(new WorkpackageStep("PhysicalPrinciples", "Physical principles"));
            _steps.Add(new WorkpackageStep("ConceptDescription", "Concept description"));
            _steps.Add(new WorkpackageStep("StructuredInformationOnTheDevice", "Structured information on the device"));
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

            var oldReview = GetSingleActiveReview();
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

            var oldReview = GetSingleActiveReview();
            var acceptedReview = oldReview.ToRejected(e.ConcludingComment, e.RejectedAt);

            _reviews.Remove(oldReview);
            _reviews.Add(acceptedReview);
        }
    }
}