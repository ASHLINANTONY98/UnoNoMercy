using UnoNoMercy.Api.Hubs;
using UnoNoMercy.Api.Services;
using UnoNoMercy.GameEngine.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

builder.Services.AddSingleton<GameManager>();
builder.Services.AddSingleton<GameService>();
builder.Services.AddSingleton<DeckService>();
builder.Services.AddSingleton<PlayerSessionService>();
builder.Services.AddHostedService<SessionCleanupService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<GameHub>("/gamehub");

app.Run();
public partial class Program
{
    
}