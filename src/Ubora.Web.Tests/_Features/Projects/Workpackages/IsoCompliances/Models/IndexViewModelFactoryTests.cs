using System;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Authorization;
using Moq;
using Ubora.Domain.Projects.IsoStandardsComplianceChecklists;
using Ubora.Web._Features.Projects.Workpackages.Steps.IsoCompliances.Models;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.Workpackages.IsoCompliances.Models
{
    public class IndexViewModelFactoryTests
    {
        private IFixture AutoFixture { get; } = new Fixture();

        [Theory]
        [InlineData(false, false)]
        [InlineData(true, true)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        public async Task Create_Maps_IsoStandards_And_Authorization(
            bool canWorkOnProjectContent,
            bool canRemoveIsoStandardFromComplianceChecklist)
        {
            var isoStandards = new []
            {
                AutoFixture.Create<IsoStandard>(),
                AutoFixture.Create<IsoStandard>()
            }.ToImmutableList();

            var aggregate = new IsoStandardsComplianceChecklist()
                .Set(x => x.IsoStandards, isoStandards);

            var claimsPrincipal = new ClaimsPrincipal();

            var authorizationServiceMock = new Mock<IAuthorizationService>();

            authorizationServiceMock
                .Setup(a => a.AuthorizeAsync(claimsPrincipal, null, Policies.CanWorkOnProjectContent))
                .ReturnsAsync(canWorkOnProjectContent ? AuthorizationResult.Success() : AuthorizationResult.Failed());

            authorizationServiceMock
                .Setup(a => a.AuthorizeAsync(claimsPrincipal, null, Policies.CanRemoveIsoStandardFromComplianceChecklist))
                .ReturnsAsync(canRemoveIsoStandardFromComplianceChecklist ? AuthorizationResult.Success() : AuthorizationResult.Failed());

            var factoryUnderTest = new IndexViewModel.Factory(authorizationServiceMock.Object);

            // Act
            var result = await factoryUnderTest.Create(claimsPrincipal, aggregate);

            // Assert
            result.IsoStandards.Should().HaveCount(2);

            using (new AssertionScope())
            {
                result.CanEditIsoStandard.Should().Be(canWorkOnProjectContent);
                result.CanRemoveIsoStandardFromComplianceChecklist.Should().Be(canRemoveIsoStandardFromComplianceChecklist);

                var entity = isoStandards.First();
                var isoStandardViewModel = result.IsoStandards.First();

                isoStandardViewModel.IsoStandard.IsoStandardId.Should().Be(entity.Id);
                isoStandardViewModel.IsoStandard.Title.Should().Be(entity.Title);
                isoStandardViewModel.IsoStandard.ShortDescription.Should().Be(entity.ShortDescription);
                isoStandardViewModel.IsoStandard.Link.Should().Be(entity.Link);

                isoStandardViewModel.CanEditIsoStandard.Should().Be(canWorkOnProjectContent);
                isoStandardViewModel.CanRemoveIsoStandardFromComplianceChecklist.Should().Be(canRemoveIsoStandardFromComplianceChecklist);

            }
        }
    }
}