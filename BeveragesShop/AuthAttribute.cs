using System.Linq;
using System.Threading.Tasks;

namespace BeveragesShop
{
    public class AdminKeyRequirement : IAuthorizationRequirement
    {
        public string AdminKey { get; set; }
        public AdminKeyRequirement(string adminKey)
        {
            AdminKey = adminKey;
        }
    }

    public class AdminKeyHandler :
     AuthorizationHandler<AdminKeyRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor = null;
        private readonly IConfiguration _configuration = null;

        public AdminKeyHandler(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        protected override Task HandleRequirementAsync(
               AuthorizationHandlerContext context,
               AdminKeyRequirement requirement)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext.Request.Query.TryGetValue(requirement.AdminKey, out StringValues value) && value.Count > 0)
            {
                var key = value[0];
                if (key == _configuration[requirement.AdminKey])

                    context.Succeed(requirement);

            }


            return Task.CompletedTask;
        }
    }
}
