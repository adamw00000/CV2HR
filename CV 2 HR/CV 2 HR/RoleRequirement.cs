using CommunityCertForT;
using CommunityCertForT.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CV_2_HR
{
    internal class RoleRequirement : IAuthorizationRequirement
    {
        public List<string> Roles { get; private set; }

        public RoleRequirement(List<string> roles)
        {
            Roles = roles;
        }
    }

    internal class RoleRequirementHandler : AuthorizationHandler<RoleRequirement>
    {
        private IConfiguration _configuration;
        private AppSettings AppSettings { get; set; }

        public RoleRequirementHandler(IConfiguration Configuration)
        {
            _configuration = Configuration;
            AppSettings = _configuration.GetSection("AppSettings").Get<AppSettings>();
        }

        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       RoleRequirement requirement)
        {
            AADGraph graph = new AADGraph(AppSettings);

            foreach (var role in requirement.Roles)
            {
                string groupName = role;
                string groupId = AppSettings.AADGroups.FirstOrDefault(g => String.Compare(g.Name, groupName) == 0).Id;
                bool isIngroup = await graph.IsUserInGroup(context.User.Claims, groupId);

                if (isIngroup)
                {
                    context.Succeed(requirement);
                    return;
                }
            }
        }
    }
}