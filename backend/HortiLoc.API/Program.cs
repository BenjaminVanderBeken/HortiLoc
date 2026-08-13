using HortiLoc.Core.Interfaces;
using HortiLoc.Core.Services;
using HortiLoc.Infrastructure.Data;
using HortiLoc.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("La chaîne de connexion MySQL est introuvable.");

builder.Services.AddSingleton(
    new DatabaseConnectionFactory(connectionString)
);

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<ClientService>();

var app = builder.Build();

app.MapControllers();

app.Run();