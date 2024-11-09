using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Newtonsoft.Json;
using BACK_END_PROJECT.API.users;

namespace BACK_END_PROJECT.API.security
{
    public class UserToken
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public HashSet<string> Roles { get; set; }

        // Construtor padrão
        public UserToken()
        {
            Id = 0;
            Name = string.Empty;
            Roles = new HashSet<string>();
        }

        // Construtor com base no usuário
        public UserToken(User user)
        {
            Id = user.Id;  // Sem necessidade de verificação de null, já que é um valor obrigatório
            Name = user.Name;
            Roles = new HashSet<string>(user.Roles.Select(r => r.Name).OrderBy(r => r));
            IsAdmin = Roles.Contains("ADMIN");
        }

        // Propriedade calculada para verificar se o usuário é um administrador
        [JsonIgnore]
        public bool IsAdmin { get; set; }

        public ClaimsPrincipal ToClaimsPrincipal()
        {
            var claims = Roles.Select(role => new Claim(ClaimTypes.Role, $"ROLE_{role}")).ToList();
            return new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
        }
    }
}