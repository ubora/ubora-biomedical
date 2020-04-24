using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.CommercialDossiers;
using Ubora.Web.Infrastructure.Extensions;
using Ubora.Web.Infrastructure.Storage;

namespace Ubora.Web._Features.Projects.Workpackages.Steps.CommercialDocumentations
{
    public class CommercialDossierPostModel
    {
        public string ProductName { get; set; }
        public string CommercialName { get; set; }
        public string DescriptionQuillDelta { get; set; }
        public IFormFile UserManual { get; set; }
        public string UserManualName { get; set; }
        public IFormFile Logo { get; set; }
        public bool HasOldLogoBeenDeleted { get; set; }
        public bool HasOldUserManualBeenRemoved { get; set; }

        public class Helper
        {
            private readonly IUboraStorageProvider _storageProvider;

            public Helper(IUboraStorageProvider storageProvider)
            {
                _storageProvider = storageProvider;
            }

            public Helper()
            {
            }

            public virtual async Task<(BlobLocation location, string fileName, long fileSize)> 
                GetUserManualInfo(CommercialDossierPostModel model, Guid projectId, UserManual existingUserManual = null)
            {
                if (model.UserManual != null)
                {
                    BlobLocation blobLocation;
                    using (var userManualStream = model.UserManual.OpenReadStream())
                    {
                        blobLocation = BlobLocations.GetCommercialDossierBlobLocation(projectId, model.UserManual.GetFileName());
                        await _storageProvider.SavePrivate(blobLocation, userManualStream);
                    }
                    return (blobLocation, model.UserManual.GetFileName(), model.UserManual.Length);
                }

                if (model.HasOldUserManualBeenRemoved)
                {
                    return (null, null, 0);
                }

                return (existingUserManual?.Location, existingUserManual?.FileName, existingUserManual?.FileSize ?? 0);
            }

            public virtual async Task<BlobLocation> GetLogoBlobLocation(CommercialDossierPostModel model, Guid projectId, BlobLocation existingLogoLocation = null)
            {
                if (model.Logo != null)
                {
                    using (var imageStream = model.Logo.OpenReadStream())
                    {
                        var blobLocation = BlobLocations.GetCommercialDossierBlobLocation(projectId, model.Logo.GetFileName());
                        await _storageProvider.SavePrivate(blobLocation, imageStream);
                        return blobLocation;
                    }
                }

                if (model.HasOldLogoBeenDeleted)
                {
                    return null;
                }

                return existingLogoLocation;
            }
        }
    }
}