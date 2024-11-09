using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace BACK_END_PROJECT.API.security
{
    public class JwtTokenFilter : IAsyncActionFilter
    {
        private readonly Jwt _jwt;
        private readonly ILogger<JwtTokenFilter> _logger;

        public JwtTokenFilter(Jwt jwt, ILogger<JwtTokenFilter> logger)
        {
            _jwt = jwt;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;

            // Extract and validate the token from the Authorization header
            var authentication = _jwt.Extract(request);

            if (authentication != null)
            {
                // Set the authenticated user in the security context
                context.HttpContext.User = authentication;
            }

            // Continue with the execution of the next middleware or action
            await next();
        }
    }
}
