using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ubora.Domain.Projects.IsoStandardsComplianceChecklists;

namespace Ubora.Web._Features.Projects.Workpackages.Steps.IsoCompliances.Models
{
    public class IndexViewModel
    {
        public IReadOnlyCollection<IsoStandardWithCheckboxViewModel> IsoStandards { get; set; }
        public bool CanEditIsoStandard { get; set; }
        public bool CanRemoveIsoStandardFromComplianceChecklist { get; set; }

        public class Factory
        {
            private readonly IAuthorizationService _authorizationService;

            public Factory(IAuthorizationService authorizationService)
            {
                _authorizationService = authorizationService;
            }

            protected Factory()
            {
            }

            public virtual async Task<IndexViewModel> Create(ClaimsPrincipal user, IsoStandardsComplianceChecklist aggregate = null)
            {
                var canEditIsoStandards = await _authorizationService.IsAuthorizedAsync(user, Policies.CanWorkOnProjectContent);
                var canRemoveIsoStandardFromComplianceChecklist = await _authorizationService.IsAuthorizedAsync(user, Policies.CanRemoveIsoStandardFromComplianceChecklist);

                return new IndexViewModel
                {
                    CanEditIsoStandard = canEditIsoStandards,
                    CanRemoveIsoStandardFromComplianceChecklist = canRemoveIsoStandardFromComplianceChecklist,

                    IsoStandards = new IsoStandardViewModel.Projection()
                        .Apply(aggregate?.IsoStandards != null ? (IEnumerable<IsoStandard>)aggregate.IsoStandards : new List<IsoStandard>())
                        .Select(isoStandard => new IsoStandardWithCheckboxViewModel
                        {
                            IsoStandard = isoStandard,
                            CanEditIsoStandard = canEditIsoStandards,
                            CanRemoveIsoStandardFromComplianceChecklist = canRemoveIsoStandardFromComplianceChecklist
                        }).ToList()
                };
            }
        }
    }
}
