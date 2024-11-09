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

        // Método assíncrono para inserir uma Role
        public async Task<Role> InsertAsync(Role role)
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
    }
}
