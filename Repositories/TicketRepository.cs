using Microsoft.EntityFrameworkCore;
using TicketsApi.Data;
using TicketsApi.Models;
using TicketsApi.Models.Enums;
using TicketsApi.Repositories.Interfaces;

namespace TicketsApi.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _context;

        public TicketRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Ticket> CreateAsync(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }

        public async Task<Ticket> ReadAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                throw new KeyNotFoundException($"Ticket com id {id} não encontrado.");
            }
            return ticket;
        }

        public async Task<IEnumerable<Ticket>> ReadAllAsync()
        {
            return await _context.Tickets
                .Where(ticket => ticket.Status != TicketStatus.Solved && ticket.Status != TicketStatus.Closed)
                .ToListAsync();
        }

        // Implementa uma busca geral baseada pelo termo no input
        public async Task<IEnumerable<Ticket>> ReadAllByTermAsync(string searchTerm)
        {
            string normalizedSearch = searchTerm.ToLower();

            var tickets = await _context.Tickets
                .Where(ticket =>
                    ticket.Title.ToLower().Contains(normalizedSearch) ||
                    ticket.Analyst.ToLower().Contains(normalizedSearch) ||
                    ticket.Client.ToLower().Contains(normalizedSearch) ||
                    ticket.NumberTicket.ToLower().Contains(normalizedSearch) ||
                    ticket.Description.ToLower().Contains(normalizedSearch) ||
                    ticket.Status.ToString().ToLower().Contains(normalizedSearch)
                )
                .ToListAsync();
            if (!tickets.Any())
            {
                throw new KeyNotFoundException($"Nenhum ticket encontrado com o termo '{normalizedSearch}'");
            }
            return tickets;
        }

        public async Task<Ticket> UpdateAsync(Ticket ticket)
        {
            var existingTicket = await _context.Tickets.FindAsync(ticket.Id);
            if (existingTicket == null)
            {
                return null;
            }

            existingTicket.Title = ticket.Title;
            existingTicket.Analyst = ticket.Analyst;
            existingTicket.Client = ticket.Client;
            existingTicket.NumberTicket = ticket.NumberTicket;
            existingTicket.Description = ticket.Description;
            existingTicket.Status = ticket.Status;
            existingTicket.SetUpdatedAt();

            _context.Entry(existingTicket).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return existingTicket;
        }

        public async Task<Ticket> DeleteAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                ticket.Status = TicketStatus.Closed;
                _context.Entry(ticket).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            return ticket;
        }
    }
}
