using System;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using Ubora.Domain.ClinicalNeeds;
using Ubora.Domain.ClinicalNeeds.Commands;
using Ubora.Domain.ClinicalNeeds.Events;
using Xunit;

namespace Ubora.Domain.Tests.ClinicalNeeds.Commands
{
    public class EditClinicalNeedCommandIntegrationTests : IntegrationFixture
    {
        [Fact]
        public void ClinicalNeed_Properties_Can_Be_Edited_All_At_Once()
        {
            var clinicalNeedId = Guid.NewGuid();
            new ClinicalNeedSeeder(this, clinicalNeedId)
                .IndicateTheClinicalNeed();

            var command = new EditClinicalNeedCommand
            {
                ClinicalNeedId = clinicalNeedId,
                Title = "testTitle",
                Description = new QuillDelta("testDesc"),
                ClinicalNeedTag = "testClinical",
                AreaOfUsageTag = "testArea",
                PotentialTechnologyTag = "testPotential",
                Keywords = "testKeywords",
                Actor = new DummyUserInfo()
            };

            // Act
            var result = Processor.Execute(command);

            // Assert
            result.IsSuccess.Should().BeTrue();

            var clinicalNeed = Session.Load<ClinicalNeed>(clinicalNeedId);

            using (new AssertionScope())
            {
                clinicalNeed.Title.Should().Be("testTitle");
                clinicalNeed.Description.Should().Be(new QuillDelta("testDesc"));
                clinicalNeed.ClinicalNeedTag.Should().Be("testClinical");
                clinicalNeed.AreaOfUsageTag.Should().Be("testArea");
                clinicalNeed.PotentialTechnologyTag.Should().Be("testPotential");
                clinicalNeed.Keywords.Should().Be("testKeywords");
            }
        }

        [Fact]
        public void Only_Necessary_Events_Are_Persisted_When_Editing_Title_Or_Description()
        {
            var clinicalNeedId = Guid.NewGuid();
            new ClinicalNeedSeeder(this, clinicalNeedId)
                .IndicateTheClinicalNeed();
            var clinicalNeed = Session.Load<ClinicalNeed>(clinicalNeedId);

            // Act & Assert (Title)
            var result = Processor.Execute(new EditClinicalNeedCommand
            {
                ClinicalNeedId = clinicalNeedId,
                Title = "newTitle",
                Description = clinicalNeed.Description,
                ClinicalNeedTag = clinicalNeed.ClinicalNeedTag,
                AreaOfUsageTag = clinicalNeed.AreaOfUsageTag,
                PotentialTechnologyTag = clinicalNeed.PotentialTechnologyTag,
                Keywords = clinicalNeed.Keywords,
                Actor = new DummyUserInfo()
            });

            result.IsSuccess.Should().BeTrue();
            var lastEvent = Session.Events.FetchStream(clinicalNeedId).Select(x => x.Data).Last();
            lastEvent.Should().BeOfType<ClinicalNeedTitleEditedEvent>();
            clinicalNeed = Session.Load<ClinicalNeed>(clinicalNeedId);
            clinicalNeed.Title.Should().Be("newTitle");

            // Act & Assert (Description)
            result = Processor.Execute(new EditClinicalNeedCommand
            {
                ClinicalNeedId = clinicalNeedId,
                Title = clinicalNeed.Title,
                Description = new QuillDelta("newDescription"),
                ClinicalNeedTag = clinicalNeed.ClinicalNeedTag,
                AreaOfUsageTag = clinicalNeed.AreaOfUsageTag,
                PotentialTechnologyTag = clinicalNeed.PotentialTechnologyTag,
                Keywords = clinicalNeed.Keywords,
                Actor = new DummyUserInfo()
            });

            result.IsSuccess.Should().BeTrue();
            lastEvent = Session.Events.FetchStream(clinicalNeedId).Select(x => x.Data).Last();
            lastEvent.Should().BeOfType<ClinicalNeedDescriptionEditedEvent>();
            clinicalNeed = Session.Load<ClinicalNeed>(clinicalNeedId);
            clinicalNeed.Description.Should().Be(new QuillDelta("newDescription"));
        }

        [Fact]
        public void Only_Neccessary_Events_Are_Persisted_When_Editing_Tags_Or_Keywords()
        {
            var clinicalNeedId = Guid.NewGuid();
            new ClinicalNeedSeeder(this, clinicalNeedId)
                .IndicateTheClinicalNeed();
            var clinicalNeed = Session.Load<ClinicalNeed>(clinicalNeedId);

            // Act & Assert (ClinicalNeedTag)
            var result = Processor.Execute(new EditClinicalNeedCommand
            {
                ClinicalNeedId = clinicalNeedId,
                Title = clinicalNeed.Title,
                Description = clinicalNeed.Description,
                ClinicalNeedTag = "newClinical",
                AreaOfUsageTag = clinicalNeed.AreaOfUsageTag,
                PotentialTechnologyTag = clinicalNeed.PotentialTechnologyTag,
                Keywords = clinicalNeed.Keywords,
                Actor = new DummyUserInfo()
            });

            result.IsSuccess.Should().BeTrue();
            var lastEvent = Session.Events.FetchStream(clinicalNeedId).Select(x => x.Data).Last();
            lastEvent.Should().BeOfType<ClinicalNeedDesignTagsEditedEvent>();
            clinicalNeed = Session.Load<ClinicalNeed>(clinicalNeedId);
            clinicalNeed.ClinicalNeedTag.Should().Be("newClinical");

            // Act & Assert (AreaOfUsageTag)
            result = Processor.Execute(new EditClinicalNeedCommand
            {
                ClinicalNeedId = clinicalNeedId,
                Title = clinicalNeed.Title,
                Description = clinicalNeed.Description,
                ClinicalNeedTag = clinicalNeed.ClinicalNeedTag,
                AreaOfUsageTag = "newArea",
                PotentialTechnologyTag = clinicalNeed.PotentialTechnologyTag,
                Keywords = clinicalNeed.Keywords,
                Actor = new DummyUserInfo()
            });

            result.IsSuccess.Should().BeTrue();
            lastEvent = Session.Events.FetchStream(clinicalNeedId).Select(x => x.Data).Last();
            lastEvent.Should().BeOfType<ClinicalNeedDesignTagsEditedEvent>();
            clinicalNeed = Session.Load<ClinicalNeed>(clinicalNeedId);
            clinicalNeed.AreaOfUsageTag.Should().Be("newArea");

            // Act & Assert (PotentialTechnologyTag)
            result = Processor.Execute(new EditClinicalNeedCommand
            {
                ClinicalNeedId = clinicalNeedId,
                Title = clinicalNeed.Title,
                Description = clinicalNeed.Description,
                ClinicalNeedTag = clinicalNeed.ClinicalNeedTag,
                AreaOfUsageTag = clinicalNeed.AreaOfUsageTag,
                PotentialTechnologyTag = "newPotential",
                Keywords = clinicalNeed.Keywords,
                Actor = new DummyUserInfo()
            });

            result.IsSuccess.Should().BeTrue();
            lastEvent = Session.Events.FetchStream(clinicalNeedId).Select(x => x.Data).Last();
            lastEvent.Should().BeOfType<ClinicalNeedDesignTagsEditedEvent>();
            clinicalNeed = Session.Load<ClinicalNeed>(clinicalNeedId);
            clinicalNeed.PotentialTechnologyTag.Should().Be("newPotential");

            // Act & Assert (Keywords)
            result = Processor.Execute(new EditClinicalNeedCommand
            {
                ClinicalNeedId = clinicalNeedId,
                Title = clinicalNeed.Title,
                Description = clinicalNeed.Description,
                ClinicalNeedTag = clinicalNeed.ClinicalNeedTag,
                AreaOfUsageTag = clinicalNeed.AreaOfUsageTag,
                PotentialTechnologyTag = clinicalNeed.PotentialTechnologyTag,
                Keywords = "newKeywords",
                Actor = new DummyUserInfo()
            });

            result.IsSuccess.Should().BeTrue();
            lastEvent = Session.Events.FetchStream(clinicalNeedId).Select(x => x.Data).Last();
            lastEvent.Should().BeOfType<ClinicalNeedDesignTagsEditedEvent>();
            clinicalNeed = Session.Load<ClinicalNeed>(clinicalNeedId);
            clinicalNeed.Keywords.Should().Be("newKeywords");
        }
    }
}
