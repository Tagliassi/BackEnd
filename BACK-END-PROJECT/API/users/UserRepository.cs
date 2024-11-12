using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BACK_END_PROJECT.API.users
{
    public class UserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Método para encontrar um usuário por email
        public User FindByEmail(string email)
        {
            return _context.Set<User>().FirstOrDefault(u => u.Email == email);
        }

        // Método para encontrar usuários por role
        public List<User> FindByRole(string role)
        {
            return _context.Set<User>()
                .Where(u => u.Roles.Any(r => r.Name == role))
                .OrderBy(u => u.Name)
                .ToList();
        }

        // Método assíncrono para encontrar usuários por role
        public async Task<List<User>> FindByRoleAsync(string role)
        {
            return await _context.Set<User>()
                .Where(u => u.Roles.Any(r => r.Name == role))
                .OrderBy(u => u.Name)
                .ToListAsync();
        }

        // Método para buscar todos os usuários
        public List<User> FindAll()
        {
            return _context.Set<User>().ToList();
        }

        // Método para salvar (inserir ou atualizar) um usuário
        public User Save(User user)
        {
            if (user.Id == 0)
            {
                _context.Set<User>().Add(user); // Inserir novo
            }
            else
            {
                _context.Set<User>().Update(user); // Atualizar existente
            }
            _context.SaveChanges();
            return user;
        }

        // Método assíncrono para salvar (inserir ou atualizar) um usuário
        public async Task<User> SaveAsync(User user)
        {
            if (user.Id == 0)
            {
                await _context.Set<User>().AddAsync(user); // Inserir novo
            }
            else
            {
                _context.Set<User>().Update(user); // Atualizar existente
            }
            await _context.SaveChangesAsync();
            return user;
        }

        // Método para deletar um usuário
        public void Delete(User user)
        {
            _context.Set<User>().Remove(user);
            _context.SaveChanges();
        }

        // Método para buscar usuário por ID
        public User FindById(long id)
        {
            return _context.Set<User>().FirstOrDefault(u => u.Id == id);
        }
    }
}
