using System;
using System.Threading.Tasks;
using FluentAssertions;
using Marten;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Moq;
using Ubora.Domain.ClinicalNeeds;
using Ubora.Web.Authorization.Requirements;
using Ubora.Web.Tests.Fakes;
using Xunit;

namespace Ubora.Web.Tests.Authorization
{
    public class IsClinicalNeedIndicatorRequirementHandlerTests
    {
        private readonly IsClinicalNeedIndicatorRequirement.Handler _handlerUnderTest;
        private readonly Mock<IQuerySession> _querySessionMock;

        public IsClinicalNeedIndicatorRequirementHandlerTests()
        {
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _querySessionMock = new Mock<IQuerySession>();

            httpContextAccessorMock
                .Setup(a => a.HttpContext.RequestServices.GetService(typeof(IQuerySession)))
                .Returns(_querySessionMock.Object);

            _handlerUnderTest = new IsClinicalNeedIndicatorRequirement.Handler(httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task Succeeds_When_User_Is_ClinicalNeed_Indicator()
        {
            var userId = Guid.NewGuid();
            var user = FakeClaimsPrincipalFactory.CreateAuthenticatedUser(userId);

            var clinicalNeed = new ClinicalNeed()
                .Set(cn => cn.Id, Guid.NewGuid())
                .Set(cn => cn.IndicatorUserId, userId);

            _querySessionMock.Setup(qs => qs.Load<ClinicalNeed>(clinicalNeed.Id)).Returns(clinicalNeed);

            var handlerContext = new AuthorizationHandlerContext(
                requirements: new[] { new IsClinicalNeedIndicatorRequirement() },
                user: user,
                resource: clinicalNeed.Id);

            // Act
            await _handlerUnderTest.HandleAsync(handlerContext);

            // Assert
            handlerContext.HasSucceeded
                .Should().BeTrue();
        }

        [Fact]
        public async Task Does_Not_Succeed_When_User_Is_Not_ClinicalNeed_Indicator()
        {
            var userId = Guid.NewGuid();
            var user = FakeClaimsPrincipalFactory.CreateAuthenticatedUser(userId);

            var clinicalNeed = new ClinicalNeed()
                .Set(cn => cn.Id, Guid.NewGuid());

            _querySessionMock.Setup(qs => qs.Load<ClinicalNeed>(clinicalNeed.Id)).Returns(clinicalNeed);

            var handlerContext = new AuthorizationHandlerContext(
                requirements: new[] { new IsClinicalNeedIndicatorRequirement() },
                user: user,
                resource: clinicalNeed.Id);

            // Act
            await _handlerUnderTest.HandleAsync(handlerContext);

            // Assert
            handlerContext.HasSucceeded
                .Should().BeFalse();
        }
    }
}
