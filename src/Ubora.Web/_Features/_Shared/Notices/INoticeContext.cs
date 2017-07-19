using System.Collections.Generic;

namespace Ubora.Web._Features._Shared.Notices
{
    public interface INoticeContext
    {
        IList<Notice> Notices { get; }
    }
}
