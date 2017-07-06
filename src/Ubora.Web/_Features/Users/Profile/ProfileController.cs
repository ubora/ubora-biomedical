using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TwentyTwenty.Storage;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Users;
using Ubora.Web.Data;
using Ubora.Web.Infrastructure.Extensions;

namespace Ubora.Web._Features.Users.Profile
{
    public class ProfileController : UboraController
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IStorageProvider _storageProvider;

        public ProfileController(ICommandQueryProcessor processor, IMapper mapper, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IStorageProvider storageProvider) : base(processor)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _storageProvider = storageProvider;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult View(Guid userId)
        {
            var userProfile = FindById<UserProfile>(userId);

            if (userProfile == null)
            {
                return new NotFoundResult();
            }

            var profileViewModel = _mapper.Map<ProfileViewModel>(userProfile);
            profileViewModel.ProfilePictureLink = _storageProvider.GetDefaultOrBlobUrl(userProfile);

            return View(profileViewModel);
        }

        [Authorize]
        public IActionResult EditProfile()
        {
            var userId = _userManager.GetUserId(User);
            var userProfile = FindById<UserProfile>(new Guid(userId));

            var userViewModel = _mapper.Map<UserProfileViewModel>(userProfile);
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
            var userId = _userManager.GetUserId(User);

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Manage");
            }

            var command = new EditUserProfileCommand
            {
                UserId = new Guid(userId),
                FirstName = model.FirstName,
                LastName = model.LastName,
                University = model.University,
                Degree = model.Degree,
                Field = model.Field,
                Biography = model.Biography,
                Skills = model.Skills,
                Role = model.Role
            };
            ExecuteUserCommand(command);

            var user = await _userManager.FindByIdAsync(userId);
            await _signInManager.RefreshSignInAsync(user);

            return RedirectToAction("Index", "Manage");
        }

        // TODO(Kaspar Kallas): Move to more specific controller (1/2)
        public IActionResult FirstTimeEditProfile(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

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
                Country = model.Country,
                Degree = model.Degree,
                Field = model.Field,
                University = model.University,
                MedicalDevice = model.MedicalDevice,
                Institution = model.Institution,
                Skills = model.Skills,
                Role = model.Role
            });

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

            var userId = _userManager.GetUserId(User);

            var filePath = model.ProfilePicture.FileName.Replace(@"\", "/");
            var fileName = Path.GetFileName(filePath);

            ExecuteUserCommand(new ChangeUserProfilePictureCommand
            {
                UserId = new Guid(userId),
                Stream = model.ProfilePicture.OpenReadStream(),
                FileName = fileName
            });

            if (!ModelState.IsValid)
            {
                return model.IsFirstTimeEditProfile ? FirstTimeEditProfile() : EditProfile();
            }

            var user = await _userManager.FindByIdAsync(userId);
            await _signInManager.RefreshSignInAsync(user);

            return RedirectToAction(model.IsFirstTimeEditProfile ? nameof(FirstTimeEditProfile) : nameof(EditProfile));
        }
    }
}