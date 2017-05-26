using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Users;
using Ubora.Web.Infrastructure;

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
            List<UserProfile> userProfiles = ExecuteQuery(new GetUserProfileListQuery()).ToList();

            var userListItemViewModels = _mapper.Map(userProfiles, new List<UserListItemViewModel>());

            return View(userListItemViewModels);
        }

        public class GetUserProfileListQuery : IQuery<IEnumerable<UserProfile>>
        {
            public class Handler : ViewModelQueryHandler<GetUserProfileListQuery, IEnumerable<UserProfile>>
            {
                public Handler(IQueryProcessor queryProcessor) : base(queryProcessor)
                {
                }

                public override IEnumerable<UserProfile> Handle(GetUserProfileListQuery query)
                {
                    var users = QueryProcessor.Find<UserProfile>();
                        
                    return users;
                }
            }
        }
    }
}
