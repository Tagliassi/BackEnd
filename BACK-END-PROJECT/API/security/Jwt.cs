using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BACK_END_PROJECT.API.security
{
    public class Jwt
    {
        private readonly string _secret;
        private readonly string _issuer;
        private readonly int _expireHours;
        private readonly int _adminExpireHours;

        public Jwt(IConfiguration configuration)
        {
            _secret = configuration["Jwt:Secret"];
            _issuer = configuration["Jwt:Issuer"];
            _expireHours = int.Parse(configuration["Jwt:ExpireHours"]);
            _adminExpireHours = int.Parse(configuration["Jwt:AdminExpireHours"]);
        }

        public string CreateToken(User user)
        {
            var userToken = new UserToken(user);

            var expiration = DateTime.UtcNow.AddHours(
                userToken.IsAdmin ? _adminExpireHours : _expireHours);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userToken.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Iss, _issuer),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("User", JsonConvert.SerializeObject(userToken))
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _issuer,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal Extract(HttpRequest request)
        {
            try
            {
                var header = request.Headers["Authorization"].FirstOrDefault();
                if (string.IsNullOrEmpty(header) || !header.StartsWith("Bearer "))
                    return null;

                var token = header.Substring(7);
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_secret);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = _issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                var jwtToken = validatedToken as JwtSecurityToken;
                if (jwtToken?.Issuer != _issuer)
                {
                    return null;
                }

                var userToken = JsonConvert.DeserializeObject<UserToken>(jwtToken?.Claims
                    .FirstOrDefault(c => c.Type == "User")?.Value);

                return userToken?.ToClaimsPrincipal();
            }
            catch (Exception ex)
            {
                // Log error
                return null;
            }
        }
    }

    public class UserToken
    {
        public long Id { get; set; }
        public bool IsAdmin { get; set; }
        public List<string> Roles { get; set; }

        public UserToken(User user)
        {
            Id = user.Id;
            IsAdmin = user.Roles.Contains("ADMIN");
            Roles = user.Roles;
        }

        public ClaimsPrincipal ToClaimsPrincipal()
        {
            var claims = Roles.Select(role => new Claim(ClaimTypes.Role, $"ROLE_{role}")).ToList();
            return new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
        }
    }
}
