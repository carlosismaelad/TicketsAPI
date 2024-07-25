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
                entity.Property(t => t.Id)
                    .ValueGeneratedOnAdd();
                entity.Property(t => t.Title).IsRequired();
                entity.Property(t => t.Analyst).IsRequired();
                entity.Property(t => t.Client).IsRequired();
                entity.Property(t => t.NumberTicket).IsRequired();
                entity.Property(t => t.Description).IsRequired().HasColumnType("text");
                entity.Property(t => t.Status).HasConversion<int>().IsRequired();
                entity.Property(t => t.CreatedAt).IsRequired().HasColumnType("datetime2");
                entity.Property(t => t.UpdatedAt).IsRequired(false).HasColumnType("datetime2");



            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id)
                    .ValueGeneratedOnAdd();
                entity.Property(u => u.Username).IsRequired();
                entity.Property(u => u.Email).IsRequired();
                entity.Property(u => u.Password).IsRequired();
                entity.Property(u => u.Role).IsRequired();
                entity.Property(u => u.Status).HasConversion<int>().IsRequired();
                entity.Property(u => u.CreatedAt).IsRequired().HasColumnType("datetime2");
                entity.Property(u => u.UpdatedAt).IsRequired(false).HasColumnType("datetime2");


            });
        }
    }
}
