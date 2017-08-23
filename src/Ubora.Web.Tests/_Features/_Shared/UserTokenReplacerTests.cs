using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using Ubora.Domain;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Users;
using Ubora.Web._Features._Shared.Tokens;
using Xunit;

namespace Ubora.Web.Tests._Features._Shared
{
    public class UserTokenReplacerTests
    {
        private readonly UserTokenReplacer _userTokenReplacer;
        private readonly Mock<IUrlHelper> _urlHelperMock;
        private readonly Mock<IQueryProcessor> _queryProcessorMock;

        public UserTokenReplacerTests()
        {
            _queryProcessorMock = new Mock<IQueryProcessor>();
            _urlHelperMock = new Mock<IUrlHelper>();
            _userTokenReplacer = new UserTokenReplacer(_queryProcessorMock.Object, _urlHelperMock.Object);
        }

        [Fact]
        public void Replaces_User_Tokens_With_Anchor_Tags()
        {
            var user1 = new UserProfile(userId: Guid.NewGuid()) { FirstName = "user1first", LastName = "user1last" };
            var user2 = new UserProfile(userId: Guid.NewGuid()) { FirstName = "user2first", LastName = "user2last" };
            var text = $"test1 {StringTokens.User(user1.UserId)} test2 {StringTokens.User(user2.UserId)} test3";

            _queryProcessorMock.Setup(x => x.FindById<UserProfile>(user1.UserId))
                .Returns(user1);

            _queryProcessorMock.Setup(x => x.FindById<UserProfile>(user2.UserId))
                .Returns(user2);

            _urlHelperMock.Setup(h => h.Action(It.Is<UrlActionContext>(
                    x => x.Action == "View" && x.Controller == "Profile" && x.Values.GetPropertyValue<Guid>("userId") == user1.UserId)))
                .Returns("user1link");

            _urlHelperMock.Setup(h => h.Action(It.Is<UrlActionContext>(
                    x => x.Action == "View" && x.Controller == "Profile" && x.Values.GetPropertyValue<Guid>("userId") == user2.UserId)))
                .Returns("user2link");

            // Act
            var result = _userTokenReplacer.ReplaceTokens(text);

            // Assert
            var expected = "test1 <a href=\"user1link\">user1first user1last</a> test2 <a href=\"user2link\">user2first user2last</a> test3";

            result.Should().Be(expected);
        }

        [Fact]
        public void Works_Without_Tokens_In_String()
        {
            // Act
            var result = _userTokenReplacer.ReplaceTokens("text");

            // Assert
            result.Should().Be("text");
        }
    }
}
