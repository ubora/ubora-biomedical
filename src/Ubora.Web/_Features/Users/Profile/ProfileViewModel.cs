namespace Ubora.Web._Features.Users.Profile
{
    public class ProfileViewModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string University { get; set; }
        public string Degree { get; set; }
        public string Field { get; set; }
        public string Biography { get; set; }
        public string Skills { get; set; }
        public string Role { get; set; }
        public string ProfilePictureLink { get; set; }
        public string FullName { get; set; }
        public bool IsDeleted { get; set; }
        public string CountryCode { get {
          return "KEN";
          }}
    }
}
