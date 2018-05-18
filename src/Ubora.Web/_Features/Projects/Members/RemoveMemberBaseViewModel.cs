using System;

namespace Ubora.Web._Features.Projects.Members
{
    public abstract class RemoveMemberBaseViewModel
    {
        public Guid MemberId { get; set; }
        public string MemberName { get; set; }
    }
}
