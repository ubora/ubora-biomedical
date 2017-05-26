using AutoMapper;
using Ubora.Domain.Infrastructure.Queries;

namespace Ubora.Web.Infrastructure
{
    public abstract class ViewModelQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        protected ViewModelQueryHandler(IQueryProcessor queryProcessor, IMapper mapper)
        {
            QueryProcessor = queryProcessor;
            Mapper = mapper;
        }

        protected IQueryProcessor QueryProcessor { get; }
        protected IMapper Mapper { get; }

        public abstract TResult Handle(TQuery query);
    }

    // EXAMPLE: TODO(Kaspar Kallas): Remove
    //public class UserListController : UboraController
    //{
    //    public UserListController(ICommandQueryProcessor processor) : base(processor)
    //    {
    //    }

    //    public IActionResult Index()
    //    {
    //        UserListViewModel model = ExecuteQuery(new GetUserProfileListQuery());

    //        //var users = _modelFactory.GetUserListItemViewModels();

    //        return View(model);
    //    }
    //}

    //public class GetUserProfileListQuery : IQuery<UserListViewModel>
    //{
    //    public class Handler : ViewModelQueryHandler<GetUserProfileListQuery, UserListViewModel>
    //    {
    //        public Handler(IQueryProcessor queryProcessor, IMapper mapper) : base(queryProcessor, mapper)
    //        {
    //        }

    //        public override UserListViewModel Handle(GetUserProfileListQuery query)
    //        {
    //            var users = QueryProcessor.Find<UserProfile>()
    //                .Select(x => Mapper.Map<UserListViewModel.UserListItemViewModel>(x));

    //            var model = new UserListViewModel
    //            {
    //                Users = users
    //            };

    //            return model;
    //        }
    //    }
    //}
}
