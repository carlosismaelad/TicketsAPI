using Microsoft.EntityFrameworkCore;
using TicketsApi.Data;
using TicketsApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuração do DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adiciona o serviço TokenService
builder.Services.AddSingleton<TokenService>();

// Adiciona o Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuração do pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Rota padrão
app.MapGet("/", () => "Você está na IzzyWay Tickets API!");

// Adicione suas rotas de API aqui

app.Run();