using System;

namespace Ubora.Web._Features.Projects.Members.Models
{
    public abstract class RemoveMemberBaseViewModel
    {
        public Guid MemberId { get; set; }
        public string MemberName { get; set; }
    }
}
