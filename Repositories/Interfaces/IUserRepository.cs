using TicketsApi.Models;

namespace TicketsApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user);
        Task<User> ReadByUsernameAsync(string username);
        Task<IEnumerable<User>> ReadAllAsync();
        Task<IEnumerable<User>> ReadAllByTermAsync(string searchTerm);
        Task<User> UpdateAsync(User user);
        Task<User> DeleteAsync(int id);
        Task<User>? AuthenticateAsync(string username, string email);
        Task<User> GetUserByEmailAsync(string email);
    }
}