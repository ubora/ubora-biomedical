using System;
using Microsoft.AspNetCore.Html;

namespace Ubora.Web._Features.Users.UserList
{
    public class UserCardViewModel
    {
        public UserCardViewModel(Guid userId, string name, string roles, string country, string university, string profilePictureUrl, IHtmlContent footerHtml = null)
        {
            UserId = userId;
            Name = name;
            Country = country;
            University = university;
            Roles = roles;
            ProfilePictureUrl = profilePictureUrl;
            FooterHtml = footerHtml;
        }
        
        public Guid UserId { get; }
        public string Name { get; }
        public string Roles { get; }
        public string ProfilePictureUrl { get; }
        public string Country { get; }
        public string University { get; }
        public IHtmlContent FooterHtml { get; }
    }
}