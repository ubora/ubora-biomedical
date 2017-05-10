using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Domain.Users;

namespace Ubora.Web._Features.Projects.Members
{
    public class MembersController : ProjectController
    {
        private readonly IQueryProcessor _queryProcessor;

        public MembersController(IQueryProcessor queryProcessor)
        {
            _queryProcessor = queryProcessor;
        }

        public IActionResult Members(Guid id)
        {
            var project = _queryProcessor.FindById<Project>(id);

            var model = new ProjectMemberListViewModel
            {
                Id = id,
                Members = project.Members.Select(m => new ProjectMemberListViewModel.Item
                {
                    UserId = m.UserId,
                    // TODO(Kaspar Kallas): Eliminate SELECT(N + 1)
                    FullName = _queryProcessor.FindById<UserProfile>(m.UserId).FullName
                })
            };

            return View(model);
        }
    }
}
