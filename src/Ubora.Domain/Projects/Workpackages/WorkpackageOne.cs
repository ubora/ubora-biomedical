using System;
using System.Linq;
using Ubora.Domain.Projects.Workpackages.Events;
using Ubora.Domain.Projects.Workpackages.Specifications;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Workpackages
{
    public class WorkpackageOne : Workpackage<WorkpackageOne>
    {
        public bool IsLocked => this.DoesSatisfy(new IsWorkpackageOneLocked());
        
        private void Apply(ProjectCreatedEvent e)
        {
            ProjectId = e.ProjectId;

            Title = "Medical need and product specification";

            _steps.Add(new WorkpackageStep(WorkpackageStepIds.DescriptionOfNeeds, "Description of Needs", Placeholders.DescriptionOfNeeds));
            _steps.Add(new WorkpackageStep(WorkpackageStepIds.DescriptionOfExistingSolutionsAndAnalysis, "Description of Existing Solutions and Analysis", Placeholders.DescriptionOfExistingSolutionsAndAnalysis));
            _steps.Add(new WorkpackageStep(WorkpackageStepIds.ProductFunctionality, "Product Functionality", Placeholders.ProductFunctionality));
            _steps.Add(new WorkpackageStep(WorkpackageStepIds.ProductPerformance, "Product Performance", Placeholders.ProductPerformance));
            _steps.Add(new WorkpackageStep(WorkpackageStepIds.ProductUsability, "Product Usability", Placeholders.ProductUsability));
            _steps.Add(new WorkpackageStep(WorkpackageStepIds.ProductSafety, "Product Safety", Placeholders.ProductSafety));
            _steps.Add(new WorkpackageStep(WorkpackageStepIds.PatientPopulationStudy, "Patient Population Study", Placeholders.PatientPopulationStudy));
            _steps.Add(new WorkpackageStep(WorkpackageStepIds.UserRequirementStudy, "User Requirement Study", Placeholders.UserRequirementStudy));
            _steps.Add(new WorkpackageStep(WorkpackageStepIds.AdditionalInformation, "Additional Information", Placeholders.AdditionalInformation));
        }

        private void Apply(WorkpackageOneStepEditedEvent e)
        {
            var step = _steps.Single(x => x.Id == e.StepId);

            step.Title = e.Title;
            step.Content = e.NewValue;
        }

        private void Apply(WorkpackageOneSubmittedForReviewEvent e)
        {
            var canApply = this.DoesSatisfy(new CanSubmitWorkpackageReview<WorkpackageOne>());
            if (!canApply)
            {
                throw new InvalidOperationException();
            }

            var newReview = WorkpackageReview.Create(e.ReviewId, e.SubmittedAt);
            _reviews.Add(newReview);
        }

        private void Apply(WorkpackageOneReviewAcceptedEvent e)
        {
            var canApply = this.DoesSatisfy(new CanWorkpackageBeAcceptedByReview<WorkpackageOne>());
            if (!canApply)
            {
                throw new InvalidOperationException();
            }

            var oldReview = GetSingleActiveReview();
            var acceptedReview = oldReview.ToAccepted(e.ConcludingComment, e.AcceptedAt);

            _reviews.Remove(oldReview);
            _reviews.Add(acceptedReview);
        }

        private void Apply(WorkpackageOneReviewRejectedEvent e)
        {
            var canApply = this.DoesSatisfy(new CanRejectWorkpackageReview<WorkpackageOne>());
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