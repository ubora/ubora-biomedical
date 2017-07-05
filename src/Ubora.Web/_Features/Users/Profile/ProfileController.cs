using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Users;

namespace Ubora.Web._Features.Users.Profile
{
    public class ProfileController : UboraController
    {
        private readonly IMapper _mapper;

        public ProfileController(ICommandQueryProcessor processor, IMapper mapper) : base(processor)
        {
            _mapper = mapper;
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
    }
}