﻿using System;
using FluentAssertions;
using Ubora.Web._Features.Projects.Workpackages;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.Workpackages
{
    public class WorkpackageListOverviewViewModelTests
    {
        // TODO(Kaspar Kallas)
        [Fact(Skip = "broken")]
        public void Model_Marks_Selected_Step()
        {
            //var selectedStepId = Guid.NewGuid().ToString();
            //var stepToBeSelected = new WorkpackageOneOverviewViewModel.Step { Id = selectedStepId };

            //var modelUnderTest = new WorkpackageListOverviewViewModel
            //{
            //    Workpackages = new[]
            //    {
            //        new WorkpackageOneOverviewViewModel
            //        {
            //            Steps = new[]
            //            {
            //                new WorkpackageOneOverviewViewModel.Step { Id = Guid.NewGuid().ToString() },
            //                new WorkpackageOneOverviewViewModel.Step { Id = Guid.NewGuid().ToString() }
            //            }
            //        },
            //        new WorkpackageOneOverviewViewModel
            //        {
            //            Steps = new[]
            //            {
            //                new WorkpackageOneOverviewViewModel.Step { Id = Guid.NewGuid().ToString() },
            //                stepToBeSelected,
            //                new WorkpackageOneOverviewViewModel.Step { Id = Guid.NewGuid().ToString() }
            //            }
            //        }
            //    }
            //};

            //// Act
            //modelUnderTest.MarkSelectedStep(selectedStepId);

            //// Assert
            //stepToBeSelected.IsSelected.Should().BeTrue();
        }
    }
}