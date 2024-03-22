using FortRoom.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Trace));

//builder.Services.AddHostedService<MainService>(); // The Main Flow .. 
//builder.Services.AddHostedService<RGBButtonService>(); // The Main Flow .. 
//builder.Services.AddHostedService<PressureMatService>(); // The Main Flow .. 
builder.Services.AddHostedService<ObstructionControlService>(); // The Main Flow .. 

// Add services to the container.

var app = builder.Build();
// Configure CORS
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
