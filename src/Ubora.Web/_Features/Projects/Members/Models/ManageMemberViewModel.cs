using System;

namespace Ubora.Web._Features.Projects.Members.Models
{
    public class ManageMemberViewModel
    {
        public ManageMemberViewModel(Guid memberUserId, bool isRemoveMemberVisible, bool isRemoveMentorVisible, bool isPromoteLeaderVisible)
        {
            MemberUserId = memberUserId;
            IsRemoveMemberVisible = isRemoveMemberVisible;
            IsRemoveMentorVisible = isRemoveMentorVisible;
            IsPromoteLeaderVisible = isPromoteLeaderVisible;
        }

        public Guid MemberUserId { get; set; }

        public bool IsRemoveMemberVisible { get; }
        public bool IsRemoveMentorVisible { get; }
        public bool IsPromoteLeaderVisible { get; }

        public bool IsManageMemberDropdownVisible => IsRemoveMemberVisible || IsRemoveMentorVisible || IsPromoteLeaderVisible;
    }
}