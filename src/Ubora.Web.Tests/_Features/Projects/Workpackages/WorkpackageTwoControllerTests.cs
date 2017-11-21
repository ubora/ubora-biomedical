using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq;
using Ubora.Domain.Projects.Candidates;
using Ubora.Domain.Projects.Candidates.Specifications;
using Ubora.Web._Features.Projects.Workpackages.Steps;
using Ubora.Web.Infrastructure.ImageServices;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.Workpackages
{
    public class WorkpackageTwoControllerTests : ProjectControllerTestsBase
    {
        private readonly WorkpackageTwoController _workpackageTwoController;
        private readonly Mock<CandidateItemViewModel.Factory> _candidateItemViewModelFactory;
        public WorkpackageTwoControllerTests()
        {
            _candidateItemViewModelFactory = new Mock<CandidateItemViewModel.Factory>(Mock.Of<ImageStorageProvider>(), Mock.Of<IMapper>());
            _workpackageTwoController = new WorkpackageTwoController(_candidateItemViewModelFactory.Object);

            SetUpForTest(_workpackageTwoController);
        }

        [Fact]
        public void Voting_Returns_Voting_View_With_Candidates()
        {
            var candidate1 = new Candidate();
            var candidate2 = new Candidate();

            QueryProcessorMock.Setup(x => x.Find(It.IsAny<IsProjectCandidateSpec>()))
                .Returns(new[] { candidate1, candidate2 });

            var candidate1ItemViewModel = new CandidateItemViewModel();
            _candidateItemViewModelFactory.Setup(x => x.Create(candidate1))
                .Returns(candidate1ItemViewModel);

            var candidate2ItemViewModel = new CandidateItemViewModel();
            _candidateItemViewModelFactory.Setup(x => x.Create(candidate2))
                .Returns(candidate2ItemViewModel);

            var expectedModel = new VotingViewModel
            {
                Candidates = new[] { candidate1ItemViewModel, candidate2ItemViewModel }.AsEnumerable()
            };

            // Act
            var result = (ViewResult)_workpackageTwoController.Voting();

            // Assert
            result.ViewName.Should().Be(nameof(WorkpackageTwoController.Voting));
            result.Model.ShouldBeEquivalentTo(expectedModel);
        }
    }
}
