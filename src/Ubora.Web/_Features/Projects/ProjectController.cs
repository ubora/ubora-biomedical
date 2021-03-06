﻿using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects;
using Ubora.Web.Authorization;
using Ubora.Web.Infrastructure.Extensions;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Features.Projects
{
    public class ProjectRouteAttribute : RouteAttribute
    {
        public ProjectRouteAttribute(string template) : base("projects/{projectId:Guid}/" + template)
        {
        }
    }

    [ProjectRoute("[controller]/[action]")]
    [Authorize(Policy = nameof(Policies.ProjectController))]
    [RedirectIfProjectDeletedOrNotFound]
    [ProjectQuickInfoToViewData]
    public abstract class ProjectController : UboraController
    {
        public Guid ProjectId => RouteData.GetProjectId();

        private Project _project;
        public Project Project => _project ?? (_project = QueryProcessor.FindById<Project>(ProjectId));

        protected void ExecuteUserProjectCommand<T>(T command, Notice successNotice) where T : UserProjectCommand
        {
            command.ProjectId = ProjectId;
            base.ExecuteUserCommand(command, successNotice);
        }

        protected new void ExecuteUserCommand<T>(T command, Notice successNotice) where T : UserCommand
        {
            base.ExecuteUserCommand(command, successNotice);
        }

        /// <summary>
        /// Disables <see cref="Ubora.Web.Authorization.Requirements.ProjectControllerRequirement.Handler"/>.
        /// </summary>
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        protected class DisableProjectControllerAuthorizationAttribute : Attribute, IDisablesProjectControllerAuthorizationFilter
        {
        }
    }
}