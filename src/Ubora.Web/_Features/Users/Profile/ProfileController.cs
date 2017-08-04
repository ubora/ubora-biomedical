using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TwentyTwenty.Storage;
using Ubora.Domain.Users;
using Ubora.Web.Data;
using Ubora.Web.Infrastructure.Extensions;
using Ubora.Web._Features.Home;

namespace Ubora.Web._Features.Users.Profile
{
    public class ProfileController : UboraController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IStorageProvider _storageProvider;

        public ProfileController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IStorageProvider storageProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _storageProvider = storageProvider;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult View(Guid userId)
        {
            var userProfile = QueryProcessor.FindById<UserProfile>(userId);

            if (userProfile == null)
            {
                return new NotFoundResult();
            }

            var profileViewModel = AutoMapper.Map<ProfileViewModel>(userProfile);
            profileViewModel.ProfilePictureLink = _storageProvider.GetDefaultOrBlobUrl(userProfile);

            return View(profileViewModel);
        }

        [Authorize]
        public IActionResult EditProfile()
        {
            var userProfile = QueryProcessor.FindById<UserProfile>(UserId);

            var userViewModel = AutoMapper.Map<UserProfileViewModel>(userProfile);
            var editProfileViewModel = new EditProfileViewModel
            {
                UserViewModel = userViewModel,
                ProfilePictureViewModel = new ProfilePictureViewModel()
            };

            return View("EditProfile", editProfileViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(UserProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Manage");
            }

            var command = new EditUserProfileCommand
            {
                UserId = this.UserId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Biography = model.Biography,
                CountryCode = model.CountryCode,
                Degree = model.Degree,
                Field = model.Field,
                University = model.University,
                MedicalDevice = model.MedicalDevice,
                Institution = model.Institution,
                Skills = model.Skills,
                Role = model.Role
            };
            ExecuteUserCommand(command);

            var user = await _userManager.FindByIdAsync(UserId.ToString());
            await _signInManager.RefreshSignInAsync(user);

            return RedirectToAction("Index", "Manage");
        }

        // TODO(Kaspar Kallas): Move to more specific controller (1/2)
        public IActionResult FirstTimeEditProfile()
        {
            var userProfile = QueryProcessor.FindById<UserProfile>(UserId);

            if (userProfile.IsFirstTimeEditedProfile)
            {
                return RedirectToAction("Index", "Home");
            }

            var firstTimeEditProfileModel = new FirstTimeEditProfileModel
            {
                ProfilePictureViewModel = new ProfilePictureViewModel
                {
                    IsFirstTimeEditProfile = true
                }
            };

            return View(nameof(FirstTimeEditProfile), firstTimeEditProfileModel);
        }

        // TODO(Kaspar Kallas): Move to more specific controller (2/2)
        [HttpPost]
        public IActionResult FirstTimeEditProfile(FirstTimeUserProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return FirstTimeEditProfile();
            }

            ExecuteUserCommand(new EditUserProfileCommand
            {
                UserId = this.UserId,
                FirstName = UserProfile.FirstName,
                LastName = UserProfile.LastName,
                Biography = model.Biography,
                CountryCode = model.CountryCode,
                Degree = model.Degree,
                Field = model.Field,
                University = model.University,
                MedicalDevice = model.MedicalDevice,
                Institution = model.Institution,
                Skills = model.Skills,
                Role = model.Role,
                IsFirstTimeEditedProfile = true
            });

            if (!ModelState.IsValid)
            {
                return FirstTimeEditProfile();
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangeProfilePicture(ProfilePictureViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return model.IsFirstTimeEditProfile ? FirstTimeEditProfile() : EditProfile();
            }
       
            var filePath = model.ProfilePicture.FileName.Replace(@"\", "/");
            var fileName = Path.GetFileName(filePath);

            ExecuteUserCommand(new ChangeUserProfilePictureCommand
            {
                UserId = UserId,
                Stream = model.ProfilePicture.OpenReadStream(),
                FileName = fileName
            });

            if (!ModelState.IsValid)
            {
                return model.IsFirstTimeEditProfile ? FirstTimeEditProfile() : EditProfile();
            }

            var user = await _userManager.FindByIdAsync(UserId.ToString());
            await _signInManager.RefreshSignInAsync(user);

            return RedirectToAction(model.IsFirstTimeEditProfile ? nameof(FirstTimeEditProfile) : nameof(EditProfile));
        }
    }
}