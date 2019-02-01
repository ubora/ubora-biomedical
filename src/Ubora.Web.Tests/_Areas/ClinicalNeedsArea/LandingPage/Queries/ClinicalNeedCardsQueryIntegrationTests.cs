using System;
using System.Linq;
using Autofac;
using FluentAssertions;
using Marten.Pagination;
using Ubora.Domain.Discussions;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Tests;
using Ubora.Domain.Tests.ClinicalNeeds;
using Ubora.Web._Areas.ClinicalNeedsArea.LandingPage.Models;
using Ubora.Web._Areas.ClinicalNeedsArea.LandingPage.Queries;
using Xunit;

namespace Ubora.Web.Tests._Areas.ClinicalNeedsArea.LandingPage.Queries
{
    public class ClinicalNeedCardsQueryIntegrationTests : IntegrationFixture
    {
        protected override void RegisterAdditional(ContainerBuilder builder)
        {
            builder.RegisterType<ClinicalNeedCardsQuery.Handler>()
                .As<IQueryHandler<ClinicalNeedCardsQuery, IPagedList<ClinicalNeedCardViewModel>>>();
        }

        [Fact]
        public void Clinical_Needs_Can_Be_Queried_As_Cards()
        {
            var clinicalNeedId1 = Guid.NewGuid();
            var clinicalNeedId2 = Guid.NewGuid();

            var indicatorId = Guid.NewGuid();
            this.Create_User(indicatorId, firstName: "testFirst", lastName: "testLast");

            new ClinicalNeedSeeder(this, clinicalNeedId1)
                .IndicateTheClinicalNeed(indicatorId)
                .EditTheClinicalNeed("edited title");

            new ClinicalNeedSeeder(this, clinicalNeedId2)
                .IndicateTheClinicalNeed();

            new DiscussionSeeder(this, clinicalNeedId1)
                .AddComment();
            var addedComment = Session.Load<Discussion>(clinicalNeedId1).Comments.Single();

            new ProjectSeeder()
                .WithRelatedClinicalNeed(clinicalNeedId1)
                .Seed(this);

            // Act
            var result = Processor.ExecuteQuery(new ClinicalNeedCardsQuery
            {
                Paging = new Paging(pageNumber: 2, pageSize: 1)
            });

            // Assert
            result.TotalItemCount.Should().Be(2);
            result.IsFirstPage.Should().BeFalse();
            result.IsLastPage.Should().BeTrue();

            var clinicalNeedCard = result.Single();

            clinicalNeedCard.Id.Should().Be(clinicalNeedId1);
            clinicalNeedCard.Title.Should().Be("edited title");
            clinicalNeedCard.IndicatorUserId.Should().Be(indicatorId);
            clinicalNeedCard.IndicatorFullName.Should().Be("testFirst testLast");

            clinicalNeedCard.LastActivityAt.Should().Be(addedComment.CommentedAt);

            clinicalNeedCard.NumberOfComments.Should().Be(1);

            clinicalNeedCard.NumberOfRelatedProjects.Should().Be(1);
        }
    }
}
