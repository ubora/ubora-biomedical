using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using FluentAssertions;
using Ubora.Web._Features._Shared;
using Ubora.Web._Features._Shared.Notices;
using Xunit;
using Newtonsoft.Json;
using Ubora.Web.Tests.Helper;
using System.Linq;

namespace Ubora.Web.Tests._Features.Shared
{
    public class TempDataWrapperTests
    {
        private TempDataWrapper _tempDataWrapper; 
        private ITempDataDictionary _tempDataDictionary;

        public TempDataWrapperTests()
        {
            _tempDataDictionary = new TestTempDataDictionary();
            _tempDataWrapper = new TempDataWrapper(_tempDataDictionary);
        }

        [Fact]
        public void Notices_Returns_Existing_Collection()
        {
            var noticesKey = nameof(TempDataWrapper.Notices);
            var expectedExistingCollection = new List<Notice>();
            _tempDataDictionary[noticesKey] = JsonConvert.SerializeObject(expectedExistingCollection);

            // Act
            var result = _tempDataWrapper.Notices;

            // Assert
            result.ShouldBeEquivalentTo(expectedExistingCollection);
        }

        [Fact]
        public void Notices_Creates_And_Returns_New_Collection_When_Nonexistent()
        {
            var noticesKey = nameof(TempDataWrapper.Notices);
            var expectedExistingCollection = JsonConvert.SerializeObject(new List<Notice>());
            _tempDataDictionary[noticesKey] = expectedExistingCollection;

            // Act
            var firstResult = _tempDataWrapper.Notices;
            var secondResult = _tempDataWrapper.Notices;

            // Assert
            firstResult.Should().NotBeNull();
            secondResult.Should().BeEquivalentTo(firstResult);
        }

        [Fact]
        public void AddNotice_Adds_Notice_To_Notices()
        {
            var noticesKey = nameof(TempDataWrapper.Notices);
            var expectedExistingCollection = JsonConvert.SerializeObject(new List<Notice>());
            _tempDataDictionary[noticesKey] = expectedExistingCollection;

            var newNotice = new Notice("expectedNotice", NoticeType.Success);

            // Act
            _tempDataWrapper.AddNotice(newNotice);

            // Assert
            var notices = _tempDataWrapper.Notices;
            notices.Single().ShouldBeEquivalentTo(newNotice);
        }
    }
}
