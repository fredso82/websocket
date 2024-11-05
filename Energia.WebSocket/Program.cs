using Energia.WebSocket;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddDbContext<EnergiaDbContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<EnergiaDbContext>();
    dbContext.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapHub<EnergiaHub>("/energiaHub");

app.MapGet("/consumos", async (EnergiaDbContext context) =>
{
    var consumos = await context.Consumos.ToListAsync();
    return Results.Ok(consumos);
}).WithName("consumos");

app.Run();

