using System.ComponentModel.DataAnnotations;
using TicketsApi.Models.Enums;

namespace TicketsApi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }

        [Required]
        public UserStatus Status { get; set; } = UserStatus.Active;
        public string CreatedAt { get; set; }

        public string? UpdatedAt { get; set; }

        public User() { }

        public User(string username, string email, string password, string role)
            : this(username, email, password, role, UserStatus.Active)
        {

        }

        public User(string username, string email, string password, string role, UserStatus status)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("O campo nome não pode ser nulo ou vazio", nameof(username));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("O campo e-mail não pode ser nulo ou vazio", nameof(email));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("O campo e-mail não pode ser nulo ou vazio", nameof(password));
            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("O campo permissão não pode ser nulo ou vazio", nameof(role));

            Username = username;
            Email = email;
            Password = password;
            Role = role;
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