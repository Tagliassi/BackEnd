using Br.Pucpr.AuthServer.Roles;
using Br.Pucpr.AuthServer.Users;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;

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

        public void Initialize()
        {
            var adminRole = _roleRepository.FindByName("ADMIN") ??
                            _roleRepository.Save(new Role
                            {
                                Name = "ADMIN",
                                Description = "System Administrator"
                            });

            if (!_roleRepository.FindByName("USER").Any())
            {
                _roleRepository.Save(new Role
                {
                    Name = "USER",
                    Description = "Premium User"
                });
            }

            if (!_userRepository.FindByRole("ADMIN").Any())
            {
                var admin = new User
                {
                    Email = _adminConfig.Email,
                    Password = _adminConfig.Password,
                    Name = _adminConfig.Name
                };

                admin.Roles.Add(adminRole);
                _userRepository.Save(admin);
                _log.LogInformation("ADMIN user created!");
            }

            _log.LogInformation("ADMIN and USER roles created!");
        }
    }
}
