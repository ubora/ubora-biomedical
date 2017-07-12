using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Projects.Members;
using Ubora.Web._Features.Users.UserList;

namespace Ubora.Web._Features.Projects.Mentors
{
    public class MentorsController : ProjectController
    {
        public IActionResult InviteMentors()
        {
            var projectMentors = QueryProcessor.ExecuteQuery(new FindProjectMentorProfilesQuery
            {
                ProjectId = this.ProjectId
            });
            var uboraMentors = QueryProcessor.ExecuteQuery(new FindUboraMentorProfilesQuery());

            var model = new MentorsViewModel
            {
                UboraMentors = uboraMentors.Select(AutoMapper.Map<UserListItemViewModel>),
                ProjectMentors = projectMentors.Select(AutoMapper.Map<UserListItemViewModel>)
            };

            return View(nameof(InviteMentors), model);
        }

        [HttpPost]
        public IActionResult InviteMentor(Guid userId)
        {
            if (!ModelState.IsValid)
            {
                return InviteMentors();
            }

            // TODO(Kaspar Kallas): Invitation with notification
            ExecuteUserProjectCommand(new AssignProjectMentorCommand
            {
                UserId = userId
            });

            if (!ModelState.IsValid)
            {
                return InviteMentors();
            }

            return RedirectToAction(nameof(InviteMentors));
        }
    }
}
