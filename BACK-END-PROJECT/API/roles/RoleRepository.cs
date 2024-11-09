using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BACK_END_PROJECT.API.roles
{
    public class RoleRepository
    {
        private readonly ApplicationDbContext _context;

        // Construtor para o repositório que recebe o contexto do banco de dados
        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Método assíncrono para salvar uma Role
        public async Task<Role> SaveAsync(Role role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }

        // Método assíncrono para listar todas as Roles, ordenadas pelo nome
        public async Task<List<Role>> FindAllAsync()
        {
            return await _context.Roles
                .OrderBy(r => r.Name)  // Ordenando pela coluna 'Name'
                .ToListAsync();
        }

        // Método assíncrono para buscar uma Role pelo nome
        public async Task<Role> FindByNameAsync(string name)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == name);
        }

        // Método não assíncrono para buscar uma Role pelo nome
        public Role FindByName(string name)
        {
            return _context.Roles
                .FirstOrDefault(r => r.Name == name);
        }
    }
}
