using BACK_END_PROJECT.API.roles;
using BACK_END_PROJECT.API.users;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace BACK_END_PROJECT.API
{
    public class Bootstrapper
    {
        private readonly RoleRepository _roleRepository;
        private readonly UserRepository _userRepository;
        private readonly AdminConfig _adminConfig;
        private readonly ILogger<Bootstrapper> _log;

        public Bootstrapper(
            RoleRepository roleRepository,
            UserRepository userRepository,
            IOptions<AdminConfig> adminConfig,
            ILogger<Bootstrapper> log)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _adminConfig = adminConfig.Value;
            _log = log;
        }

        // Método assíncrono para inicializar as roles e usuários
        public async Task InitializeAsync()
        {
            // Procurando pela role ADMIN, ou criando-a se não existir
            var adminRole = await _roleRepository.FindByNameAsync("ADMIN");
            if (adminRole == null)
            {
                adminRole = await _roleRepository.SaveAsync(new Role
                {
                    Name = "ADMIN",
                    Description = "System Administrator"
                });
            }

            // Verificando se a role USER existe, caso contrário, cria-a
            var userRole = await _roleRepository.FindByNameAsync("USER");
            if (userRole == null)
            {
                await _roleRepository.SaveAsync(new Role
                {
                    Name = "USER",
                    Description = "Premium User"
                });
            }

            // Verificando se existe algum usuário com a role ADMIN, caso contrário, cria um
            var adminUser = await _userRepository.FindByRoleAsync("ADMIN");
            if (adminUser == null || !adminUser.Any())
            {
                var admin = new User
                {
                    Email = _adminConfig.Email,
                    Password = _adminConfig.Password,
                    Name = _adminConfig.Name
                };

                admin.Roles.Add(adminRole);
                await _userRepository.SaveAsync(admin); // Salvando o usuário ADMIN
                _log.LogInformation("ADMIN user created!");
            }

            _log.LogInformation("ADMIN and USER roles created!");
        }
    }
}
