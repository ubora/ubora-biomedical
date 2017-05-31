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

            var profileViewModel = _mapper.Map(userProfile, new ProfileViewModel());

            return View(profileViewModel);
        }
    }
}