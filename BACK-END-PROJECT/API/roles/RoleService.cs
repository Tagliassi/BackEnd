using System.Collections.Generic;
using System.Threading.Tasks;

namespace BACK_END_PROJECT.API.roles
{
    public class RoleService
    {
        private readonly RoleRepository _roleRepository;

        public RoleService(RoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        // Método para inserir uma nova role
        public async Task<Role> InsertAsync(Role role)
        {
            return await _roleRepository.SaveAsync(role); // Supondo que o método SaveAsync seja assíncrono
        }

        // Método para listar todas as roles
        public async Task<List<Role>> FindAllAsync()
        {
            return await _roleRepository.FindAllAsync(); // Supondo que o método FindAllAsync seja assíncrono
        }

        // Método para buscar uma role pelo nome
        public async Task<Role> FindByNameAsync(string name)
        {
            return await _roleRepository.FindByNameAsync(name); // Supondo que o método FindByNameAsync seja assíncrono
        }
    }
}
