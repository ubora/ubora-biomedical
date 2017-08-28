using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System.Collections.Generic;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Features._Shared
{
    public class NoticeQueue
    {
        public const string TempDataKey = nameof(NoticeQueue) + "_6954A13F-D23D-407B-9EB9-D199C2BF3763";

        private readonly ITempDataDictionary _tempData;
        private Queue<Notice> _innerQueue;

        public NoticeQueue(ITempDataDictionary tempData)
        {
            _tempData = tempData;
            _innerQueue = GetExistingOrCreateNew();
        }

        public int Count => _innerQueue.Count;

        public void Enqueue(Notice notice)
        {
            _innerQueue.Enqueue(notice);

            UpdateTempData();
        }

        public Notice Dequeue()
        {
            var notice = _innerQueue.Dequeue();

            UpdateTempData();

            return notice;
        }

        private Queue<Notice> GetExistingOrCreateNew()
        {
            var existingNotices = _tempData[TempDataKey];
            if (existingNotices == null)
            {
                return _innerQueue = new Queue<Notice>();
            }
            return _innerQueue = JsonConvert.DeserializeObject<Queue<Notice>>(existingNotices.ToString());
        }

        private void UpdateTempData()
        {
            _tempData[TempDataKey] = JsonConvert.SerializeObject(_innerQueue);
        }
    }
}