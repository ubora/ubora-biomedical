using System;
using Autofac;
using FluentAssertions;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Infrastructure.Marten;
using Ubora.Domain.Projects;
using Xunit;

namespace Ubora.Domain.Tests.Projects
{
    public class UpdateProjectTests : IntegrationFixture
    {
        public UpdateProjectTests()
        {
            StoreOptions(new UboraStoreOptions().Configuration());
        }

        [Fact]
        public void Updates_Project()
        {
            var processor = Container.Resolve<ICommandProcessor>();

            var projectId = Guid.NewGuid();
            processor.Execute(new CreateProjectCommand
            {
                NewProjectId = projectId,
                Actor = new UserInfo(Guid.NewGuid(), "")
            });

            var command = new UpdateProjectCommand
            {
                ProjectId = projectId,
                Title = "title",
                ClinicalNeedTags = "clinicalNeedTags",
                AreaOfUsageTags = "areaOfUsageTags",
                PotentialTechnologyTags = "potentialTechnologyTags",
                DescriptionOfNeed = "descriptionOfNeed",
                DescriptionOfExistingSolutionsAndAnalysis = "descriptionOfExistingSolutionsAndAnalysis",
                ProductFunctionality = "productFunctionality",
                ProductPerformance = "productPerformance",
                ProductUsability = "productUsability",
                ProductSafety = "productSafety",
                PatientPopulationStudy = "patientPopulationStudy",
                UserRequirementStudy = "userRequirementStudy",
                AdditionalInformation = "additionalInformation",
                GmdnTerm = "gmdnTerm",
                Actor = new UserInfo(Guid.NewGuid(), "")
            };

            // Act
            processor.Execute(command);

            // Assert
            var project = Session.Load<Project>(projectId);

            project.Id.Should().Be(projectId);
            project.Title.Should().Be("title");
            project.ClinicalNeedTags.Should().Be("clinicalNeedTags");
            project.AreaOfUsageTags.Should().Be("areaOfUsageTags");
            project.PotentialTechnologyTags.Should().Be("potentialTechnologyTags");
            project.DescriptionOfNeed.Should().Be("descriptionOfNeed");
            project.DescriptionOfExistingSolutionsAndAnalysis.Should().Be("descriptionOfExistingSolutionsAndAnalysis");
            project.ProductFunctionality.Should().Be("productFunctionality");
            project.ProductPerformance.Should().Be("productPerformance");
            project.ProductUsability.Should().Be("productUsability");
            project.ProductSafety.Should().Be("productSafety");
            project.PatientPopulationStudy.Should().Be("patientPopulationStudy");
            project.UserRequirementStudy.Should().Be("userRequirementStudy");
            project.AdditionalInformation.Should().Be("additionalInformation");
            project.GmdnTerm.Should().Be("gmdnTerm");
        }
    }
}
