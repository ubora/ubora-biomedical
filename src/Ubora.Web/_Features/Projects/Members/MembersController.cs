using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects;
using Ubora.Domain.Users;

namespace Ubora.Web._Features.Projects.Members
{
    public class MembersController : ProjectController
    {
        private readonly ICommandQueryProcessor _processor;

        public MembersController(ICommandQueryProcessor processor)
        {
            _processor = processor;
        }

        public IActionResult Members(Guid id)
        {
            var project = _processor.FindById<Project>(id);

            var model = new ProjectMemberListViewModel
            {
                Id = id,
                Members = project.Members.Select(m => new ProjectMemberListViewModel.Item
                {
                    UserId = m.UserId,
                    // TODO(Kaspar Kallas): Eliminate SELECT(N + 1)
                    FullName = _processor.FindById<UserProfile>(m.UserId).FullName
                })
            };

            return View(model);
        }

        public IActionResult Invite(Guid id)
        {
            // TODO: Authorize

            var model = new InviteProjectMemberViewModel { ProjectId = id };

            return View(model);
        }

        [HttpPost]
        public IActionResult Invite(InviteProjectMemberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }



            return null;
        }
    }
}
