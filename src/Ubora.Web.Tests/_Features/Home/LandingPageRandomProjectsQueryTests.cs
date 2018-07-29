using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Baseline;
using FluentAssertions;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Tests;
using Ubora.Domain.Users;
using Ubora.Domain.Users.Queries;
using Ubora.Web.Infrastructure;
using Ubora.Web._Features.Home;
using Xunit;

namespace Ubora.Web.Tests._Features.Home
{
    public class LandingPageRandomProjectsQueryTests : IntegrationFixture
    {
        protected override void RegisterAdditional(ContainerBuilder builder)
        {
            builder.RegisterModule(new WebAutofacModule(useSpecifiedPickupDirectory: true));
            builder.RegisterType<TestFindUboraMentorProfilesQueryHandler>()
                .As<IQueryHandler<FindUboraMentorProfilesQuery, IReadOnlyCollection<UserProfile>>>();
        }

        [Fact]
        public void Returns_Four_Random_Not_Draft_Projects_That_Have_Pictures()
        {
            var expectedProjectIds = new List<Guid>();
            10.Times(x => expectedProjectIds.Add(Guid.NewGuid()));

            foreach (var expectedProjectId in expectedProjectIds)
            {
                CreateNotDraftProjectWithPicture(expectedProjectId);
            }

            5.Times(x => CreateDraftProjectWithPicture(Guid.NewGuid()));
            5.Times(x => CreateNotDraftProjectWithoutPicture(Guid.NewGuid()));

            // Act
            var result1 = Processor.ExecuteQuery(new LandingPageRandomProjectsQuery());
            var result2 = Processor.ExecuteQuery(new LandingPageRandomProjectsQuery());
            var result3 = Processor.ExecuteQuery(new LandingPageRandomProjectsQuery());
            var result4 = Processor.ExecuteQuery(new LandingPageRandomProjectsQuery());

            // Assert
            var allResultProjectIds = new[] {result1, result2, result3, result4}.SelectMany(result => result.Select(project => project.Id)).ToArray();

            AssertResultsAreOfExpectedProjects();
            AssertResultCountsAreThree();
            AssertResultsAreNotAllSame(); // i.e are random
            
            void AssertResultsAreOfExpectedProjects()
            {
                allResultProjectIds.Should().BeSubsetOf(expectedProjectIds);
            }

            void AssertResultCountsAreThree()
            {
                result1.Count.Should().Be(3);
                result2.Count.Should().Be(3);
                result3.Count.Should().Be(3);
                result4.Count.Should().Be(3);
            }

            void AssertResultsAreNotAllSame()
            {
                var result1ProjectIds = new[] {result1, result1, result1, result1}.SelectMany(result => result.Select(project => project.Id));

                result1ProjectIds.Should().NotBeEquivalentTo(allResultProjectIds);
            }
        }

        private void CreateNotDraftProjectWithPicture(Guid projectId)
        {
            CreateNotDraftProjectWithoutPicture(projectId);
            this.Upload_Dummy_Project_Image(projectId);
        }

        private void CreateNotDraftProjectWithoutPicture(Guid projectId)
        {
            this.Create_Project(projectId);
            this.Submit_Workpackage_One_For_Review(projectId);
            this.Accept_Workpackage_One_Review(projectId);
        }

        private void CreateDraftProjectWithPicture(Guid projectId)
        {
            this.Create_Project(projectId);
            this.Upload_Dummy_Project_Image(projectId);
        }
    }
}