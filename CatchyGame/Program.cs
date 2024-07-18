using CatchyGame.Data;
using CatchyGame.Service;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CatchyGame.Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Services 
builder.Services.AddSingleton<IScoreRepository, ScoreRepository>(); // The Main Flow .. 
//builder.Services.AddSingleton<IHostedService, RandomRGBScenarioWithGameMode>(); // The Main Flow .. 


//builder.Services.AddHostedService<SenarioService>(); // The Main Flow .. 
//builder.Services.AddHostedService<SneakSenarioService>(); // The Main Flow .. 
//builder.Services.AddHostedService<SparkRGBScenario>(); // The Main Flow .. 
//builder.Services.AddHostedService<RandomRGBScenario>(); // The Main Flow .. 
builder.Services.AddHostedService<RandomRGBScenarioWithGameMode>(); // The Main Flow .. 
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters
.Add(new JsonStringEnumConverter()));
builder.Services.AddLogging(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Trace));
var app = builder.Build();
app.UseCors("corsapp");

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
