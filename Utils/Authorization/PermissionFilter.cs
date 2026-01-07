using Leoni.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Leoni.Utils.Authorization
{
    public class PermissionFilter : IAsyncAuthorizationFilter
    {
        private readonly IpermissionService _permissionService;

        public PermissionFilter(IpermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var endpoint = context.HttpContext.GetEndpoint();
            var permissionAttribute = endpoint?.Metadata.GetMetadata<PermissionAttribute>();

            if (permissionAttribute == null)
                return; 

            var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            bool hasPermission = await _permissionService.HasPermissions(userIdClaim, permissionAttribute.RequiredAll, permissionAttribute.Permissions);

            if (!hasPermission)
            {
                context.Result = new ForbidResult();
            }
        }

    }
}
