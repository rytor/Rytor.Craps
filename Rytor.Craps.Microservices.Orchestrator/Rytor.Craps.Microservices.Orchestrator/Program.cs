using Microsoft.Extensions.Configuration;
using Rytor.Craps.Microservices.Orchestrator.Interfaces;
using Rytor.Craps.Microservices.Orchestrator.Services;

var builder = WebApplication.CreateBuilder(args);

using IHost host = Host.CreateDefaultBuilder(args).Build();

// Add services to the container.
IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IRegistrationService, RegistrationService>(x => new RegistrationService(configuration.GetValue<string>("Instruments:Account"), configuration.GetValue<string>("Instruments:Balance"), x.GetService<ILoggerFactory>()));
builder.Services.AddSingleton<IBalanceService, BalanceService>(x => new BalanceService(configuration.GetValue<string>("Instruments:Balance"), x.GetService<ILoggerFactory>()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
