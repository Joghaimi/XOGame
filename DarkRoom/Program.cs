using DarkRoom.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));
// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters
.Add(new JsonStringEnumConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<MainService>();
builder.Services.AddHostedService<DarkRoomService>();
//builder.Services.AddHostedService<LedMatrixService>();
builder.Services.AddLogging(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Trace));


var app = builder.Build();
app.UseCors("corsapp");
// Configure CORS
//app.UseCors(options =>
//{
//    options.AllowAnyOrigin()
//           .AllowAnyMethod()
//           .AllowAnyHeader();
//});
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
