using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Users;
using Ubora.Domain.Users.Commands;
using Ubora.Web.Data;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Web.Infrastructure.Storage;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Features.Users.Profile
{
    public class ProfileController : UboraController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ImageStorageProvider _imageStorageProvider;

        public ProfileController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ImageStorageProvider imageStorageProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _imageStorageProvider = imageStorageProvider;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ViewProfile(Guid userId, [FromServices]ProfileViewModel.Factory modelFactory)
        {
            var userProfile = QueryProcessor.FindById<UserProfile>(userId);
            if (userProfile == null)
            {
                return NotFound();
            }

            var model = modelFactory.Create(userProfile);
            return View(model);
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
        [Authorize]
        public async Task<IActionResult> EditProfile(UserProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Manage");
            }

            ExecuteUserCommand(new EditUserProfileCommand
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
            }, Notice.Success(SuccessTexts.ProfileEdited));

            if (!ModelState.IsValid)
            {
                Notices.NotifyOfError("Failed to change profile!");

                return RedirectToAction("Index", "Manage");
            }

            var user = await _userManager.FindByIdAsync(UserId.ToString());
            await _signInManager.RefreshSignInAsync(user);

            return RedirectToAction("Index", "Manage");
        }

        // TODO(Kaspar Kallas): Move to more specific controller (1/2)
        [Authorize]
        public IActionResult FirstTimeEditProfile(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var firstTimeEditProfileModel = new FirstTimeEditProfileModel
            {
                FirstTimeUserProfileViewModel = new FirstTimeUserProfileViewModel(),
                ProfilePictureViewModel = new ProfilePictureViewModel
                {
                    IsFirstTimeEditProfile = true
                }
            };

            return View(nameof(FirstTimeEditProfile), firstTimeEditProfileModel);
        }

        // TODO(Kaspar Kallas): Move to more specific controller (2/2)
        [HttpPost]
        [Authorize]
        public IActionResult FirstTimeEditProfile(FirstTimeUserProfileViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return FirstTimeEditProfile(returnUrl);
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
                Role = model.Role
            }, Notice.None("Probably no reason to show notice here because it's just one of the many steps of the registration process."));

            if (!ModelState.IsValid)
            {
                return FirstTimeEditProfile(returnUrl);
            }

            return RedirectToLocal(returnUrl);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangeProfilePicture(ProfilePictureViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return model.IsFirstTimeEditProfile ? FirstTimeEditProfile() : EditProfile();
            }

            var blobLocation = BlobLocations.GetUserProfilePictureLocation(UserId, model.ImageName);

            using (var profilePictureStream = model.ProfilePicture.OpenReadStream())
            {
                await _imageStorageProvider.SaveImageAsync(profilePictureStream, blobLocation);
            }

            ExecuteUserCommand(new ChangeUserProfilePictureCommand
            {
                BlobLocation = blobLocation
            }, Notice.Success(SuccessTexts.ProfilePictureUploaded));

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