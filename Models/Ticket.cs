using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        public string CreatedAt { get; set; }

        public string? UpdatedAt { get; set; }

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
            CreatedAt = GetBrazilTime();
        }

        public void SetUpdatedAt()
        {
            UpdatedAt = GetBrazilTime();
        }

        private string GetBrazilTime()
        {
            var BrazilTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            var FormatedBrazilTimeZone = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, BrazilTimeZone);
            return FormatedBrazilTimeZone.ToString("dd/MM/yyyy HH:mm");
        }
    }
}