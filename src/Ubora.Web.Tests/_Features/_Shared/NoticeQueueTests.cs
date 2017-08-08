using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Newtonsoft.Json;
using Ubora.Web.Tests.Helper;
using Ubora.Web._Features._Shared;
using Ubora.Web._Features._Shared.Notices;
using Xunit;

namespace Ubora.Web.Tests._Features._Shared
{
    public class NoticeQueueTests
    {
        private readonly NoticeQueue _noticeQueue;
        private readonly TempDataDictionary _tempData;

        public NoticeQueueTests()
        {
            _tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
            _noticeQueue = new NoticeQueue(_tempData);
        }

        [Fact]
        public void Enqueue_Persists_Notices_In_TempData()
        {
            var notices = new[]
            {
                new Notice("123", NoticeType.Info),
                new Notice("456", NoticeType.Error),
                new Notice("789", NoticeType.Success)
            };

            // Acts
            foreach (var notice in notices)
            {
                _noticeQueue.Enqueue(notice);
            }

            // Assert
            var noticesFromTempdata = GetNoticesFromTempData(_tempData);

            noticesFromTempdata.ShouldBeEquivalentTo(notices);
        }

        [Fact]
        public void Dequeue_Returns_First_Notice_From_Queue_And_Unpersists_It_From_TempData()
        {
            var expectedNotice = new Notice("expected", NoticeType.Error);
            var notices = new[]
            {
                expectedNotice,
                new Notice("456", NoticeType.Error),
                new Notice("789", NoticeType.Success)
            };

            foreach (var notice in notices)
            {
                _noticeQueue.Enqueue(notice);
            }

            // Act
            var dequeuedNotice = _noticeQueue.Dequeue();

            // Assert
            dequeuedNotice.ShouldBeEquivalentTo(expectedNotice);

            var noticesFromTempdata = GetNoticesFromTempData(_tempData);
            noticesFromTempdata.Length.Should().Be(2);
            noticesFromTempdata.Should().NotContain(x => x.Text == expectedNotice.Text);
        }

        private Notice[] GetNoticesFromTempData(ITempDataDictionary tempData)
        {
            var noticesJsonFromTempdata = tempData[NoticeQueue.TempDataKey].ToString();

            var noticesFromTempdata = JsonConvert.DeserializeObject<Notice[]>(noticesJsonFromTempdata);

            return noticesFromTempdata;
        }
    }
}
