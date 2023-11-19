using LoRModule.Models;
using LoRModule.Repositories;
using LoRModule.Repositories.Impl;
using LoRModule.Services;
using LoRModule.Services.Impl;

var builder = WebApplication.CreateBuilder(args);

// Add Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Injection dependencies
builder.Services.AddTransient<IPlayerService, PlayerService>();
builder.Services.AddTransient<IMatchService, MatchService>();
builder.Services.AddTransient<ISummonerRepository, SummonerRepository>();
builder.Services.AddTransient<IMatchRepository, MatchRepository>();

//Add MongoDB
builder.Services.Configure<LoRDatabaseSettings>(
    builder.Configuration.GetSection("LorDatabase"));

builder.Services.Configure<LoRDatabaseSettings>(settings =>
    settings.ConnectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING") ??
                                throw new MissingFieldException(
                                    "Missing Environment Variable for mongoDB connection string"));


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