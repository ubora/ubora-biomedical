using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Users;
using Ubora.Web.Data;

namespace Ubora.Web._Features.Users.Profile
{
    public class ProfileController : UboraController
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ProfileController(ICommandQueryProcessor processor, IMapper mapper, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : base(processor)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
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
                UserViewModel = userViewModel
            };

            return View("EditProfile", editProfileViewModel);
        }

        [HttpPost]
        public IActionResult EditProfile(UserProfileViewModel model)
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

            return RedirectToAction("Index", "Manage");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangeProfilePicture(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return EditProfile();
            }

            var userId = _userManager.GetUserId(User);

            // GetFileName method that don't work well. Don't trust too the FileName(display different results)! 
            var getCorrectFileName = model.ProfilePicture.FileName.Substring(model.ProfilePicture.FileName.LastIndexOf(@"\") + 1);

            ExecuteUserCommand(new ChangeUserProfilePictureCommand()
            {
                UserId = new Guid(userId),
                Stream = model.ProfilePicture.OpenReadStream(),
                FileName = getCorrectFileName
            });

            if (!ModelState.IsValid)
            {
                return EditProfile();
            }

            var user = await _userManager.FindByIdAsync(userId);
            await _signInManager.RefreshSignInAsync(user);

            return RedirectToAction("EditProfile");
        }

        // TODO(Kaspar Kallas): Move to more specific controller (1/2)
        public IActionResult FirstTimeEditProfile(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        // TODO(Kaspar Kallas): Move to more specific controller (2/2)
        [HttpPost]
        public IActionResult FirstTimeEditProfile(FirstTimeEditProfileModel model, string returnUrl = null)
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
                University = model.University,
                Degree = model.Degree,
                Field = model.Field,
                Biography = model.Biography,
                Skills = model.Skills,
                Role = model.Role
            });

            if (!ModelState.IsValid)
            {
                return FirstTimeEditProfile(returnUrl);
            }

            return RedirectToLocal(returnUrl);
        }
    }
}