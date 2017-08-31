using FluentAssertions;
using Moq;
using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Web._Features._Shared.Tokens;
using Ubora.Web._Features.Projects.History;
using Ubora.Web._Features.Projects.History._Base;
using Xunit;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ubora.Web.Tests._Features.Projects.History
{
    public class EventViewModelFactoryMediatorTests
    {
        private readonly GeneralEventViewModel.Factory _generalFactory;

        public EventViewModelFactoryMediatorTests()
        {
            _generalFactory = new GeneralEventViewModel.Factory(Mock.Of<TokenReplacerMediator>());
        }

        [Fact]
        public void Create_Returns_ViewModel_From_Correct_Notification_Factory()
        {
            var correctFactoryMock = new Mock<EventViewModelFactory<TestUboraEvent, TestEventViewModel>>
            {
                CallBase = true
            };

            var factories = new[]
            {
                Mock.Of<IEventViewModelFactory>(x => x.CanCreateFor(typeof(TestUboraEvent)) == false),
                correctFactoryMock.Object,
                Mock.Of<IEventViewModelFactory>(x => x.CanCreateFor(typeof(TestUboraEvent)) == false)
            };

            var userInfo = new UserInfo(Guid.NewGuid(), "");
            var uboraEvent = new TestUboraEvent(userInfo);
            var expectedViewModel = new TestEventViewModel();

            var timestamp = DateTimeOffset.Now;

            correctFactoryMock.Setup(x => x.Create(uboraEvent, timestamp))
                .Returns(expectedViewModel);

            var mediator = new EventViewModelFactoryMediator(factories, _generalFactory);

            // Act
            var result = mediator.Create(uboraEvent, timestamp);

            // Assert
            result.Should().Be(expectedViewModel);
        }

        [Fact]
        public void Create_Tries_To_Return_ViewModel_From_General_Factory_When_Could_Not_Find_Concrete_Factory()
        {
            var factories = new[]
            {
                Mock.Of<IEventViewModelFactory>(x => x.CanCreateFor(It.IsAny<Type>()) == false),
                Mock.Of<IEventViewModelFactory>(x => x.CanCreateFor(It.IsAny<Type>()) == false),
            };

            var userInfo = new UserInfo(Guid.NewGuid(), "");
            var uboraEvent = new TestUboraEvent(userInfo);
            var timestamp = DateTimeOffset.Now;

            var mediator = new EventViewModelFactoryMediator(factories, _generalFactory);

            // Act
            var result = mediator.Create(uboraEvent, timestamp);

            // Assert
            result.Should().BeOfType<GeneralEventViewModel>();
        }

        public class TestUboraEvent : UboraEvent
        {
            public TestUboraEvent(UserInfo initiatedBy) : base(initiatedBy)
            {
            }

            public override string GetDescription()
            {
                return "";
            }
        }

        public class TestEventViewModel : IEventViewModel<TestUboraEvent>
        {
            public IHtmlContent GetPartialView(IHtmlHelper htmlHelper)
            {
                throw new NotImplementedException();
            }
        }
    }
}
