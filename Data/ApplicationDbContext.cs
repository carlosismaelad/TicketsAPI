using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketsApi.Models;

namespace TicketsApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Title).IsRequired();
                entity.Property(t => t.Analyst).IsRequired();
                entity.Property(t => t.Client).IsRequired();
                entity.Property(t => t.NumberTicket).IsRequired();
                entity.Property(t => t.Description).HasColumnType("text").IsRequired();
                entity.Property(t => t.Status).IsRequired().HasConversion<int>();
                entity.Property(t => t.CreatedAt).IsRequired();
                entity.Property(t => t.UpdatedAt).IsRequired(false);

            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Username).IsRequired();
                entity.Property(u => u.Email).IsRequired();
                entity.Property(u => u.Password).IsRequired();
                entity.Property(u => u.Role).IsRequired();
                entity.Property(u => u.Status).IsRequired().HasConversion<int>();
                entity.Property(u => u.CreatedAt).IsRequired();
                entity.Property(u => u.UpdatedAt).IsRequired(false);
            });

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "Batman",
                    Email = "batman@example.com",
                    Password = HashPassword("AdminPassword123"),
                    Role = "Admin",
                    CreatedAt = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm")
                },
                new User
                {
                    Id = 2,
                    Username = "Robin",
                    Email = "robin@example.com",
                    Password = HashPassword("UserPassword123"),
                    Role = "Default",
                    CreatedAt = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm")
                }
            );
        }

        protected string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

    }
}