using System;
using System.Threading.Tasks;
using Ubora.Domain;
using Ubora.Domain.Projects.CommercialDossiers;
using Ubora.Web.Infrastructure;
using Ubora.Web.Infrastructure.Storage;

namespace Ubora.Web._Features.Projects.Workpackages.Steps.CommercialDocumentations
{
    public class CommercialDossierViewModel : CommercialDossierPostModel
    {
        public string LogoUrl { get; set; }
        public string DescriptionHtml { get; set; }
        public bool DoesDescriptionHaveContent { get; set; }

        public class Factory
        {
            private readonly QuillDeltaTransformer _quillDeltaTransformer;
            private readonly IUboraStorageProvider _storageProvider;

            public Factory(QuillDeltaTransformer quillDeltaTransformer, IUboraStorageProvider storageProvider)
            {
                _quillDeltaTransformer = quillDeltaTransformer;
                _storageProvider = storageProvider;
            }

            public Factory()
            {
            }

            public virtual async Task<CommercialDossierViewModel> Create(CommercialDossier commercialDossier)
            {
                return new CommercialDossierViewModel
                {
                    ProductName = commercialDossier.ProductName,
                    CommercialName = commercialDossier.CommercialName,
                    DescriptionHtml = await _quillDeltaTransformer.ConvertQuillDeltaToHtml(commercialDossier.Description),
                    DescriptionQuillDelta = await _quillDeltaTransformer.SanitizeQuillDeltaForEditing(commercialDossier.Description),
                    DoesDescriptionHaveContent = commercialDossier.Description != new QuillDelta(),
                    LogoUrl = commercialDossier.Logo != null ? _storageProvider.GetReadUrl(commercialDossier.Logo, DateTime.UtcNow.AddSeconds(10)) : null,
                    UserManualName = commercialDossier.UserManual?.FileName
                };
            }
        }
    }
}