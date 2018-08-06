using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Ubora.Web.Authorization.Requirements
{
    public class PandocServiceIpRequirement : IAuthorizationRequirement
    {
        public class Handler : AuthorizationHandler<PandocServiceIpRequirement>
        {
            private readonly IHttpContextAccessor _contextAccessor;
            private readonly IOptions<Pandoc> _appSettings;

            public Handler(IHttpContextAccessor contextAccessor, IOptions<Pandoc> appSettings)
            {
                _contextAccessor = contextAccessor;
                _appSettings = appSettings;
            }
            
            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PandocServiceIpRequirement requirement)
            {
                var clientIpAddress = _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

                if (!_appSettings.Value.IsUnsafeHttp)
                {
                    var isHttps = _contextAccessor.HttpContext.Request.IsHttps;
                    
                    if (clientIpAddress == _appSettings.Value.Ip && isHttps)
                    {
                        context.Succeed(requirement);
                    } 
                }
                else
                {
                    if (clientIpAddress == _appSettings.Value.Ip)
                    {
                        context.Succeed(requirement);
                    } 
                }
                  
                return Task.CompletedTask;
            }
        }
    }
}