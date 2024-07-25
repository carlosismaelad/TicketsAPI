using TicketsApi.Models;

namespace TicketsApi.Repositories.Interfaces
{
    public interface ITicketRepository
    {
        Task<Ticket> CreateAsync(Ticket ticket);
        Task<Ticket> ReadAsync(int id);
        Task<IEnumerable<Ticket>> ReadAllAsync();
        Task<IEnumerable<Ticket>> ReadAllByTermAsync(string searchTerm);
        Task<Ticket> UpdateAsync(Ticket ticket);
        Task<Ticket> DeleteAsync(int id);
    }
}