using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

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
            Id = user.Id ?? -1L;
            Name = user.Name;
            Roles = new HashSet<string>(user.Roles.Select(r => r.Name).OrderBy(r => r));
        }

        // Propriedade calculada para verificar se o usuário é um administrador
        [JsonIgnore]
        public bool IsAdmin => Roles.Contains("ADMIN");
    }
}
