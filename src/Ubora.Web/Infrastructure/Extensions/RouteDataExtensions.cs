using System;
using Microsoft.AspNetCore.Routing;

namespace Ubora.Web.Infrastructure.Extensions
{
    public static class RouteDataExtensions
    {
        public static Guid GetProjectId(this RouteData routeData)
        {
            var projectIdFromRoute = routeData.Values["projectId"] as string;
            Guid.TryParse(projectIdFromRoute, out Guid projectId);

            return projectId;
        }
    }
}