using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Users;

namespace Ubora.Web._Features.Users.UserList
{
    public class UserListController : UboraController
    {
        private readonly IMapper _mapper;

        public UserListController(ICommandQueryProcessor processor, IMapper mapper) : base(processor)
        {
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var userProfiles = Find<UserProfile>();
            var userListItemViewModels = _mapper.Map(userProfiles, new List<UserListItemViewModel>());

            return View(userListItemViewModels);
        }
    }
}
