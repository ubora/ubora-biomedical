using System;
using System.Threading.Tasks;
using Marten;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Ubora.Domain.ClinicalNeeds;
using Ubora.Web.Services;

namespace Ubora.Web.Authorization.Requirements
{
    public class IsClinicalNeedIndicatorRequirement : IAuthorizationRequirement
    {
        public class Handler : AuthorizationHandler<IsClinicalNeedIndicatorRequirement, Guid>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;
            }

            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsClinicalNeedIndicatorRequirement requirement, Guid clinicalNeedId)
            {
                var querySession = _httpContextAccessor.HttpContext.RequestServices.GetService<IQuerySession>();

                if (context.User.Identity.IsAuthenticated)
                {
                    var clinicalNeed = querySession.Load<ClinicalNeed>(clinicalNeedId);
                    
                    if (clinicalNeed?.IndicatorUserId == context.User.GetId())
                    {
                        context.Succeed(requirement);
                    }
                }

                return Task.CompletedTask;
            }
        }
    }
}
