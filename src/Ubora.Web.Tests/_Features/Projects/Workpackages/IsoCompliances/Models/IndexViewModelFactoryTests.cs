using System;
using System.Collections.Immutable;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Ubora.Domain.Projects.IsoStandardsCompliances;
using Ubora.Web._Features.Projects.Workpackages.Steps.IsoCompliances.Models;
using Xunit;

namespace Ubora.Web.Tests._Features.Projects.Workpackages.IsoCompliances.Models
{
    public class IndexViewModelFactoryTests
    {
        private IFixture AutoFixture { get; } = new Fixture();

        [Fact]
        public void Create_Maps_IsoStandards()
        {
            var isoStandards = new []
            {
                AutoFixture.Create<IsoStandard>(),
                AutoFixture.Create<IsoStandard>()
            }.ToImmutableList();

            var aggregate = new IsoStandardsComplianceAggregate()
                .Set(x => x.IsoStandards, isoStandards);

            var factoryUnderTest = new IndexViewModel.Factory();

            // Act
            var result = factoryUnderTest.Create(aggregate);

            // Assert
            result.IsoStandards.Should().HaveCount(2);

            using (new AssertionScope())
            {
                var entity = isoStandards.First();
                var viewModel = result.IsoStandards.First();

                viewModel.IsoStandardId.Should().Be(entity.Id);
                viewModel.Title.Should().Be(entity.Title);
                viewModel.ShortDescription.Should().Be(entity.ShortDescription);
                viewModel.Link.Should().Be(entity.Link);
            }
        }
    }
}