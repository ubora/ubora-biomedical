using System;
using FluentAssertions;
using Ubora.Web._Features.Projects.Workpackages;
using Ubora.Web._Features.Projects.Workpackages.WorkpackageOne;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.Workpackages
{
    public class WorkpackageListOverviewViewModelTests
    {
        [Fact]
        public void Model_Marks_Selected_Step()
        {
            var selectedStepId = Guid.NewGuid();
            var stepToBeSelected = new WorkpackageOneOverviewViewModel.Step { Id = selectedStepId };

            var modelUnderTest = new WorkpackageListOverviewViewModel
            {
                Workpackages = new[]
                {
                    new WorkpackageOneOverviewViewModel
                    {
                        Steps = new[]
                        {
                            new WorkpackageOneOverviewViewModel.Step { Id = Guid.NewGuid() },
                            new WorkpackageOneOverviewViewModel.Step { Id = Guid.NewGuid() }
                        }
                    },
                    new WorkpackageOneOverviewViewModel
                    {
                        Steps = new[]
                        {
                            new WorkpackageOneOverviewViewModel.Step { Id = Guid.NewGuid() },
                            stepToBeSelected,
                            new WorkpackageOneOverviewViewModel.Step { Id = Guid.NewGuid() }
                        }
                    }
                }
            };

            // Act
            modelUnderTest.MarkSelectedStep(selectedStepId);

            // Assert
            stepToBeSelected.IsSelected.Should().BeTrue();
        }
    }
}