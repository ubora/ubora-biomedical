using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System.Collections.Generic;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Features._Shared
{
    /// <summary>
    /// Wraps TempData for strong typing.
    /// </summary>
    public class TempDataWrapper : INoticeContext
    {
        private readonly ITempDataDictionary _tempData;

        public TempDataWrapper(ITempDataDictionary tempData)
        {
            _tempData = tempData;
        }

        public IList<Notice> Notices => GetOrCreateNotices();

        public void AddNotice(Notice notice)
        {
            var existingNotices = Notices;
            existingNotices.Add(notice);

            var key = nameof(Notices);
            _tempData[key] = JsonConvert.SerializeObject(existingNotices);
        }

        private IList<Notice> GetOrCreateNotices()
        {
            var key = nameof(Notices);

            var existingNotices = _tempData[key];
            if (existingNotices != null)
            {
                return JsonConvert.DeserializeObject<List<Notice>>(_tempData[key].ToString());
            }

            var newNotices = new List<Notice>();
            _tempData[key] = JsonConvert.SerializeObject(newNotices);
            return newNotices;
        }
    }
}
