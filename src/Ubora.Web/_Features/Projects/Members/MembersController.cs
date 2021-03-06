﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Ubora.Domain.Projects;
using Ubora.Web.Services;
using Microsoft.AspNetCore.Identity;
using Ubora.Web.Data;
using Ubora.Domain.Projects.Members.Commands;
using Ubora.Web.Authorization.Requirements;
using Ubora.Web.Infrastructure.Extensions;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Web._Features.Projects.Members.Models;
using Ubora.Web._Features._Shared.Notices;
using Ubora.Domain.Projects.Members.Specifications;
using Ubora.Domain.Users.Queries;

namespace Ubora.Web._Features.Projects.Members
{
    [ProjectRoute("[controller]")]
    public class MembersController : ProjectController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ImageStorageProvider _imageStorageProvider;

        public MembersController(SignInManager<ApplicationUser> signInManager, ImageStorageProvider imageStorageProvider)
        {
            _signInManager = signInManager;
            _imageStorageProvider = imageStorageProvider;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Members()
        {
            var isProjectMember = (await AuthorizationService.AuthorizeAsync(User, null, new IsProjectMemberRequirement())).Succeeded;
            var isAuthenticated = (await AuthorizationService.AuthorizeAsync(User, Policies.IsAuthenticatedUser)).Succeeded;

            var canRemoveProjectMember = (await AuthorizationService.AuthorizeAsync(User, Policies.CanRemoveProjectMember)).Succeeded;
            var canRemoveProjectMentor = (await AuthorizationService.AuthorizeAsync(User, Policies.CanRemoveProjectMentor)).Succeeded;
            var canPromoteMember = (await AuthorizationService.AuthorizeAsync(User, Policies.CanPromoteMember)).Succeeded;
            var memberListItemViewModels = new List<ProjectMemberListViewModel.Item>();

            var projectMemberGroups = Project.Members.GroupBy(m => m.UserId);
            var projectMemberUserProfiles = QueryProcessor.ExecuteQuery(new GetProjectUserProfiles { ProjectId = ProjectId });
            foreach (var userProfile in projectMemberUserProfiles)
            {
                var projectMemberGroup = projectMemberGroups.FirstOrDefault(g => g.Key == userProfile.UserId);

                var itemModel = new ProjectMemberListViewModel.Item
                {
                    UserId = userProfile.UserId,
                    IsProjectLeader = projectMemberGroup.Any(x => x.IsLeader),
                    IsProjectMentor = projectMemberGroup.Any(x => x.IsMentor),
                    IsCurrentUser = (isAuthenticated && this.UserId == userProfile.UserId),
                    FullName = userProfile.FullName,
                    ProfilePictureUrl = _imageStorageProvider.GetDefaultOrBlobUrl(userProfile),
                    CanRemoveProjectMentor = (await AuthorizationService.AuthorizeAsync(User, projectMemberGroup.FirstOrDefault(x => x.IsMentor), Policies.CanRemoveProjectMentor)).Succeeded
            };
                memberListItemViewModels.Add(itemModel);
            }

            var isProjectLeader = isAuthenticated && Project.Members.Any(x => x.UserId == UserId && x.IsLeader);

            var model = new ProjectMemberListViewModel
            {
                Id = ProjectId,
                Items = memberListItemViewModels,
                IsProjectMember = isAuthenticated && isProjectMember,
                IsProjectLeader = isProjectLeader
            };

            return View(nameof(Members), model);
        }

        [Route(nameof(Invite))]
        public IActionResult Invite()
        {
            return View();
        }

        [HttpPost]
        [Route(nameof(Invite))]
        public IActionResult Invite(InviteProjectMemberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Invite();
            }

            ExecuteUserProjectCommand(new InviteMemberToProjectCommand
            {
                InvitedMemberEmail = model.Email
            }, Notice.Success(SuccessTexts.ProjectMemberInvited));

            if (!ModelState.IsValid)
            {
                return Invite();
            }

            return RedirectToAction(nameof(Members));
        }

        [Route(nameof(Join))]
        [DisableProjectControllerAuthorization]
        [Authorize(Policies.CanJoinProject)]
        public IActionResult Join(Guid projectId)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                var returnUrl = Url.Action("Join", "Members", new { projectId = projectId });

                return RedirectToAction("SignInSignUp", "Account", new { returnUrl = returnUrl });
            }

            var project = QueryProcessor.FindById<Project>(projectId);

            var model = new JoinProjectViewModel
            {
                UserId = User.GetId(),
                ProjectId = projectId,
                ProjectName = project.Title
            };

            return View(model);
        }

        [HttpPost]
        [Route(nameof(Join))]
        [DisableProjectControllerAuthorization]
        [Authorize(Policies.CanJoinProject)]
        public IActionResult Join(JoinProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ExecuteUserProjectCommand(new JoinProjectCommand(), Notice.Success(SuccessTexts.RequestToJoinProjectSent));

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("Dashboard", "Dashboard", new { });
        }

        [Route(nameof(RemoveMember))]
        [Authorize(Policies.CanRemoveProjectMember)]
        public IActionResult RemoveMember(Guid memberId)
        {
            var removeMember = new RemoveMemberViewModel
            {
                MemberId = memberId,
                MemberName = QueryProcessor.FindById<UserProfile>(memberId).FullName
            };

            return View(removeMember);
        }

        [HttpPost]
        [Route(nameof(RemoveMember))]
        [Authorize(Policies.CanRemoveProjectMember)]
        public IActionResult RemoveMember(RemoveMemberViewModel removeMemberViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(removeMemberViewModel);
            }

            ExecuteUserProjectCommand(new RemoveMemberFromProjectCommand
            {
                UserId = removeMemberViewModel.MemberId
            }, Notice.Success(SuccessTexts.ProjectMemberRemoved));

            if (!ModelState.IsValid)
            {
                return View(removeMemberViewModel);
            }

            return RedirectToAction(nameof(Members));
        }

        [Route(nameof(RemoveMentor))]
        [DisableProjectControllerAuthorization]
        public async Task<IActionResult> RemoveMentor(Guid memberId)
        {
            var projectMember = Project.GetMembers(new HasUserIdSpec(memberId)).FirstOrDefault(x => x.IsMentor);
            var canRemoveProjectMentor = (await AuthorizationService.AuthorizeAsync(User, projectMember, Policies.CanRemoveProjectMentor)).Succeeded;
            if (!canRemoveProjectMentor)
            {
                return Forbid();
            }

            var removeMentorViewModel = new RemoveMentorViewModel
            {
                MemberId = memberId,
                MemberName = QueryProcessor.FindById<UserProfile>(memberId).FullName
            };

            return View(removeMentorViewModel);
        }

        [HttpPost]
        [DisableProjectControllerAuthorization]
        [Route(nameof(RemoveMentor))]
        public async Task<IActionResult> RemoveMentor(RemoveMentorViewModel removeMentorViewModel)
        {
            var projectMember = Project.GetMembers(new HasUserIdSpec(removeMentorViewModel.MemberId)).FirstOrDefault(x => x.IsMentor);
            var canRemoveProjectMentor = (await AuthorizationService.AuthorizeAsync(User, projectMember, Policies.CanRemoveProjectMentor)).Succeeded;
            if (!canRemoveProjectMentor)
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return View(removeMentorViewModel);
            }

            ExecuteUserProjectCommand(new RemoveMemberFromProjectCommand
            {
                UserId = removeMentorViewModel.MemberId
            }, Notice.Success(SuccessTexts.ProjectMentorRemoved));

            if (!ModelState.IsValid)
            {
                return View(removeMentorViewModel);
            }

            return RedirectToAction(nameof(Members));
        }

        [Route(nameof(PromoteMember))]
        [DisableProjectControllerAuthorization]
        [Authorize(Policies.CanPromoteMember)]
        public IActionResult PromoteMember(Guid memberId)
        {
            var promoteMemberViewModel = new PromoteMemberViewModel
            {
                MemberId = memberId,
                MemberName = QueryProcessor.FindById<UserProfile>(memberId).FullName
            };

            return View(promoteMemberViewModel);
        }

        [HttpPost]
        [DisableProjectControllerAuthorization]
        [Authorize(Policies.CanPromoteMember)]
        [Route(nameof(PromoteMember))]
        public IActionResult PromoteMember(PromoteMemberViewModel promoteMemberViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(promoteMemberViewModel);
            }

            ExecuteUserProjectCommand(new PromoteProjectLeaderCommand
            {
                UserId = promoteMemberViewModel.MemberId
            }, Notice.Success(SuccessTexts.ProjectLeaderPromoted));

            if (!ModelState.IsValid)
            {
                return View(promoteMemberViewModel);
            }

            return RedirectToAction(nameof(Members));
        }

        [Route(nameof(Leave))]
        public IActionResult Leave()
        {
            return View(nameof(Leave));
        }

        [HttpPost]
        [Route(nameof(Leave))]
        public IActionResult LeaveProject()
        {
            if (!ModelState.IsValid)
            {
                return Leave();
            }

            ExecuteUserProjectCommand(new RemoveMemberFromProjectCommand
            {
                UserId = UserId
            }, Notice.Success(SuccessTexts.LeftFromProject));

            if (!ModelState.IsValid)
            {
                return Leave();
            }

            return RedirectToAction("Dashboard", "Dashboard", new { });
        }
    }
}