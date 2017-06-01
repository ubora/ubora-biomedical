using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Users;
using Ubora.Domain.Notifications;

namespace Ubora.Web._Features.Projects.Members
{
    public class MembersController : ProjectController
    {
        public MembersController(ICommandQueryProcessor processor) : base(processor)
        {
        }

        [Route(nameof(Members))]
        public IActionResult Members()
        {
            var model = new ProjectMemberListViewModel
            {
                Id = ProjectId,
                Members = Project.Members.Select(m => new ProjectMemberListViewModel.Item
                {
                    UserId = m.UserId,
                    // TODO(Kaspar Kallas): Eliminate SELECT(N + 1)
                    FullName = FindById<UserProfile>(m.UserId).FullName
                })
            };

            return View(model);
        }

        public IActionResult Invite()
        {
            var model = new InviteProjectMemberViewModel { ProjectId = ProjectId };

            return View(model);
        }

        [HttpPost]
        public IActionResult Invite(InviteProjectMemberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Invite();
            }

            ExecuteUserProjectCommand(new InviteMemberToProjectCommand
            {
                InvitedMemberEmail = model.Email
            });

            if (!ModelState.IsValid)
            {
                return Invite();
            }

            return RedirectToAction(nameof(Members), new { id = model.ProjectId });
        }
    }
}
