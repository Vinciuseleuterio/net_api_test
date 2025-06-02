using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
DotNetEnv.Env.Load();

// Realiza a conex�o ao banco relacionando os contextos devidos
builder.Services.AddDbContext<ApplicationContext>(o =>
    o.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING_DB")));

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