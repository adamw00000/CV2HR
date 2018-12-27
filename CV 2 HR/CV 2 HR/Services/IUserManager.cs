using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CV_2_HR.Services
{
    public interface IUserManager
    {
        string GetUserId();
        ClaimsPrincipal GetUser();
        Task<AuthorizationResult> AuthorizeUserAsync(string role);
    }
}
