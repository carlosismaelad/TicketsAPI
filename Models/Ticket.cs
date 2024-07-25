using System.ComponentModel.DataAnnotations;
using TicketsApi.Models.Enums;

namespace TicketsApi.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Analyst { get; set; }

        [Required]
        public string Client { get; set; }

        [Required]
        public string NumberTicket { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public TicketStatus Status { get; set; } = TicketStatus.New;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Ticket(string title, string analyst, string client, string numberTicket, string description)
            : this(title, analyst, client, numberTicket, description, TicketStatus.New)
        {
        }

        public Ticket(string title, string analyst, string client, string numberTicket, string description, TicketStatus status)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("O campo título não pode ser nulo ou vazio", nameof(title));

            if (string.IsNullOrWhiteSpace(analyst))
                throw new ArgumentException("O campo analista não pode ser nulo ou vazio", nameof(analyst));

            if (string.IsNullOrWhiteSpace(client))
                throw new ArgumentException("O campo cliente não pode ser nulo ou vazio", nameof(client));

            if (numberTicket.Length <= 0)
                throw new ArgumentException("O campo numero do ticket não pode ser nulo ou vazio", nameof(numberTicket));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("O campo descrição não pode ser nulo ou vazio", nameof(description));

            Title = title;
            Analyst = analyst;
            Client = client;
            NumberTicket = numberTicket;
            Description = description;
            Status = status;
            CreatedAt = DateTime.UtcNow;
        }

        public void SetUpdatedAt()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}