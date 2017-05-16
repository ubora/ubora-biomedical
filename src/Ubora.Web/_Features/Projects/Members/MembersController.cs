using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Members;
using Ubora.Domain.Users;

namespace Ubora.Web._Features.Projects.Members
{
    [Authorize(Policy = nameof(Policies.FromProject))]
    public class MembersController : ProjectController
    {
        public MembersController(ICommandQueryProcessor processor) : base(processor)
        {
        }

        [Route("members")]
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
            // TODO: Authorize

            var model = new InviteProjectMemberViewModel { ProjectId = ProjectId };

            return View(model);
        }

        [HttpPost]
        public IActionResult Invite(InviteProjectMemberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ExecuteCommand(new InviteMemberToProjectCommand
            {
                ProjectId = model.ProjectId,
                UserId = model.UserId.Value,
                UserInfo = this.UserInfo
            });

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction(nameof(Members), new { id = model.ProjectId });
        }
    }
}
