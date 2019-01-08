using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CV2HR.Services
{
    public class HttpContextUserManager : IUserManager
    {
        private readonly IHttpContextAccessor _context;
        private readonly IAuthorizationService _authorizationService;

        public HttpContextUserManager(IHttpContextAccessor context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        public string GetUserId()
        {
            return _context.HttpContext.User?.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;
        }

        public ClaimsPrincipal GetUser()
        {
            return _context.HttpContext.User;
        }

        public async Task<AuthorizationResult> AuthorizeUserAsync(string role)
        {
            return await _authorizationService.AuthorizeAsync(GetUser(), role);
        }
    }
}
