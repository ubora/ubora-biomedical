using System;
using System.Linq;
using Ubora.Domain.Projects.WorkpackageSpecifications;
using Ubora.Domain.Projects.WorkpackageTwos;

namespace Ubora.Domain.Projects.WorkpackageOnes
{
    public class WorkpackageOne : Workpackage<WorkpackageOne>
    {
        public bool IsLocked => this.DoesSatisfy(new IsLocked());

        private void Apply(ProjectCreatedEvent e)
        {
            ProjectId = e.Id;

            Title = "Design and prototyping";

            _steps.Add(new WorkpackageStep("Description Of Need", Placeholders.DescriptionOfNeed));
            _steps.Add(new WorkpackageStep("Description Of Existing Solutions And Analysis", Placeholders.DescriptionOfExistingSolutionsAndAnalysis));
            _steps.Add(new WorkpackageStep("Product Functionality", Placeholders.ProductFunctionality));
            _steps.Add(new WorkpackageStep("Product Performance", Placeholders.ProductPerformance));
            _steps.Add(new WorkpackageStep("Product Usability", Placeholders.ProductUsability));
            _steps.Add(new WorkpackageStep("Product Safety", Placeholders.ProductSafety));
            _steps.Add(new WorkpackageStep("Patient Population Study", Placeholders.PatientPopulationStudy));
            _steps.Add(new WorkpackageStep("User Requirement Study", Placeholders.UserRequirementStudy));
            _steps.Add(new WorkpackageStep("Additional Information", Placeholders.AdditionalInformation));

            IsVisible = true;
        }

        private void Apply(WorkpackageOneStepEditedEvent e)
        {
            var canApply = this.DoesSatisfy(new CanWorkpackageOneStepBeEdited(e.StepId));
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

            var newReview = WorkpackageReview.Create();
            _reviews.Add(newReview);
        }

        private void Apply(WorkpackageOneAcceptedByReviewEvent e)
        {
            var canApply = this.DoesSatisfy(new CanWorkpackageBeAcceptedByReview<WorkpackageOne>());
            if (!canApply)
            {
                throw new InvalidOperationException();
            }

            var oldReview = GetSingleActiveReview();
            var acceptedReview = oldReview.ToAccepted(e.ConcludingComment);

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

            var oldReview = GetSingleActiveReview();
            var acceptedReview = oldReview.ToRejected(e.ConcludingComment);

            _reviews.Remove(oldReview);
            _reviews.Add(acceptedReview);
        }
    }
}