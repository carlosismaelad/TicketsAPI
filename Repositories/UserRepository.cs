using Microsoft.EntityFrameworkCore;
using TicketsApi.Data;
using TicketsApi.Models;
using TicketsApi.Models.Enums;
using TicketsApi.Repositories.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace TicketsApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> CreateAsync(User user)
        {
            user.Password = BC.HashPassword(user.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> ReadAsync(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Username == username);
            if (user == null)
            {
                throw new KeyNotFoundException($"Usuário {username} não encontrado.");
            }
            return user;
        }

        public async Task<IEnumerable<User>> ReadAllAsync()
        {
            return await _context.Users
                .Where(user => user.Status != UserStatus.Disable)
                .ToListAsync();
        }

        // Implementa uma busca geral baseada pelo termo no input
        public async Task<IEnumerable<User>> ReadAllByTermAsync(string searchTerm)
        {
            // Normaliza o searchTerm para busca case insensitive
            string normalizedSearch = searchTerm.ToLower();

            var users = await _context.Users
                .Where(user =>
                    user.Username.ToLower().Contains(normalizedSearch) ||
                    user.Email.ToLower().Contains(normalizedSearch) ||
                    user.Status.ToString().ToLower().Contains(normalizedSearch)


                )
                .ToListAsync();
            if (!users.Any())
            {
                throw new KeyNotFoundException($"Nenhum usuário encontrado com o termo '{normalizedSearch}'");
            }
            return users;
        }

        public async Task<User> UpdateAsync(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null)
            {
                return null;
            }
            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.Password = BC.HashPassword(user.Password);
            existingUser.Role = user.Role;
            existingUser.Status = user.Status;
            existingUser.SetUpdatedAt();

            _context.Entry(existingUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return existingUser;
        }

        public async Task<User> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.Status = UserStatus.Disable;
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            return user;
        }

        // Método para teste de autenticação
        public async Task<User> AuthenticateAsync(string email, string password)
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            if (user != null && BC.Verify(password, user.Password))
            {
                return user;
            }
            return null;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

    }
}