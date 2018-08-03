using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Projects.Repository;
using Ubora.Web.Infrastructure.Storage;

namespace Ubora.Web._Features.Projects.Repository
{
    [ProjectRoute("Repository")]
    [DisableProjectControllerAuthorization]
    public class ClipboardController : ProjectController
    {
        private readonly IUboraStorageProvider _uboraStorageProvider;

        public ClipboardController(IUboraStorageProvider uboraStorageProvider)
        {
            _uboraStorageProvider = uboraStorageProvider;
        }

        //Don't change route name because the name is already in use from markdown editor
        [Route("DownloadFile")]
        [Authorize(Policy = nameof(Policies.CanCopyFileToClipboard))]
        public IActionResult CopyFile(Guid fileId)
        {
            var file = QueryProcessor.FindById<ProjectFile>(fileId);
            if (file == null)
            {
                throw new InvalidOperationException();
            }

            var blobSasUrl = _uboraStorageProvider.GetReadUrl(file.Location, DateTime.UtcNow.AddSeconds(5));

            return Redirect(blobSasUrl);
        }
    }
}