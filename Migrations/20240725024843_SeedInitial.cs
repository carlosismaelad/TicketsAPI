using Microsoft.EntityFrameworkCore.Migrations;
using TicketsApi.Models.Enums;
using BC = BCrypt.Net.BCrypt;

#nullable disable

namespace TicketsApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Analyst = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Client = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberTicket = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
            migrationBuilder.InsertData(
            table: "Users",
            columns: new[] { "Username", "Email", "Password", "Role", "Status", "CreatedAt" },
            values: new object[,]
            {
                { "Batman", "batman@example.com", BC.HashPassword("AdminPassword123"), (int)UserStatus.Active, "Admin", DateTime.UtcNow },
                { "Robin", "robin@example.com", BC.HashPassword("UserPassword123"), (int)UserStatus.Active, "Default", DateTime.UtcNow }
            });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
