﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Users.Profile
{
    public class ProfilePictureViewModel
    {
        [Required(ErrorMessage = "Please select an image to upload first!")]
        [IsImage]
        [FileSize(1000000, "The limit for profile image is 1 MB!")]
        public IFormFile ProfilePicture { get; set; }
        public bool IsFirstTimeEditProfile { get; set; }
        private string FilePath => ProfilePicture?.FileName.Replace(@"\\", "/");
        public string ImageName => Path.GetFileName(FilePath);
        public string ImageNameWithoutExtension => Path.GetFileNameWithoutExtension(FilePath);
    }
}
