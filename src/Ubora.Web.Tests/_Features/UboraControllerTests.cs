using Ubora.Web._Features;
using Ubora.Web._Features._Shared.Notices;
using Xunit;
using FluentAssertions;
using Ubora.Web.Tests.Helper;
using Ubora.Web._Features._Shared;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace Ubora.Web.Tests._Features
{
    public class UboraControllerTests
    {
        public class TestController : UboraController
        {
            public void TestShowNotice(Notice notice)
            {
                ShowNotice(notice);
            }
        }

        [Fact]
        public void ShowNotice_Adds_Notice_To_TempData()
        {
            var controller = new TestController();
            SetTempData(controller);

            var notice = new Notice("test", NoticeType.Success);

            // Act
            controller.TestShowNotice(notice);

            // Assert
            var actualNotices = controller.TempDataWrapper.Notices;
            actualNotices.Count.Should().Be(1);
            actualNotices.Single().ShouldBeEquivalentTo(notice);
        }

        private void SetTempData(TestController controller)
        {
            var tempDataDictionary = new TestTempDataDictionary();
            var noticesKey = nameof(TempDataWrapper.Notices);
            tempDataDictionary[noticesKey] = JsonConvert.SerializeObject(new List<Notice>());

            controller.TempData = tempDataDictionary;
        }
    }
}
