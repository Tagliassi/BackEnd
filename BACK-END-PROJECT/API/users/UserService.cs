using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using BACK_END_PROJECT.API.users.responses;
using BACK_END_PROJECT.API.roles;
using BACK_END_PROJECT.API.errors;
using BACK_END_PROJECT.API.security;
using System.Threading.Tasks;

namespace BACK_END_PROJECT.API.users
{
    public class UserService
    {
        private readonly UserRepository _repository;
        private readonly RoleRepository _roleRepository;
        private readonly Jwt _jwt;
        private static readonly ILogger<UserService> _log = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<UserService>();

        public UserService(UserRepository repository, RoleRepository roleRepository, Jwt jwt)
        {
            _repository = repository;
            _roleRepository = roleRepository;
            _jwt = jwt;
        }

        public User Insert(User user)
        {

            if (_repository.FindByEmail(user.Email) != null)
                throw new BadRequestException($"Usuário com email {user.Email} já existe!");

            return _repository.Save(user);
        }

        public List<User> List(SortDir sortDir, string role)
        {
            if (!string.IsNullOrEmpty(role))
            {
                
                return sortDir == SortDir.ASC
                    ? _repository.FindByRole(role).ToList() : _repository.FindByRole(role).OrderByDescending(u => u.Id).ToList();
            }
            else
            {
                return sortDir == SortDir.ASC
                    ? _repository.FindAll().ToList()
                    : _repository.FindAll().OrderByDescending(u => u.Id).ToList();
            }
        }

        public User FindByIdOrNull(long id) => _repository.FindById(id);

        public User Delete(long id)
        {
            var user = _repository.FindById(id);
            if (user == null) return null;

            if (user.Roles.Any(r => r.Name == "ADMIN") && _repository.FindByRole("ADMIN").Count() == 1)
                throw new BadRequestException("Não é possível excluir o último administrador!");

            _repository.Delete(user);
            return user;
        }

        public User Update(long id, string name)
        {
            var user = _repository.FindById(id) ?? throw new NotFoundException($"Usuário {id} não encontrado!");

            if (user.Name == name)
                return null;

            user.Name = name;
            return _repository.Save(user);
        }

        public bool AddRole(long id, string roleName)
        {
            var user = _repository.FindById(id) ?? throw new NotFoundException("Usuário não encontrado");

            if (user.Roles.Any(r => r.Name == roleName))
                return false;

            var role = _roleRepository.FindByName(roleName) ?? throw new BadRequestException("Invalid role name!");
            
            user.Roles.Add(role);
            _repository.Save(user);
            return true;
        }

        public LoginResponse Login(string email, string password)
        {
            var user = _repository.FindByEmail(email);
            if (user == null || user.Password != password)
                return null;

            _log.LogInformation($"User logged in. id={user.Id} name={user.Name}");
            
            // Criação correta da resposta de login
            return new LoginResponse(_jwt.CreateToken(user), new UserResponse(user));
        }

    }
}
