using JobHunter.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("JobHunter")
    ?? throw new InvalidOperationException("Connection string 'JobHunter' is not configured.");

builder.Services.AddInfrastructure(connectionString);

var app = builder.Build();
app.Run();
