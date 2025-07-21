using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NotesApp.Application.Services;
using NotesApp.Domain.Interfaces;
using NotesApp.Infrastructure.Data;
using NotesApp.Infrastructure.Repositories;
using FluentValidation;
using NotesApp.Application.Validators;
using NotesApp.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "NotesApp",
        Version = "v1"
    });
});

DotNetEnv.Env.Load();

// Register database context

builder.Services.AddDbContext<ApplicationContext>(o =>
    o.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING_DB")));

// Register Services and Repositories
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<NoteService>();
builder.Services.AddScoped<INoteRepository, NoteRepository>();

builder.Services.AddScoped<GroupService>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();

// Register FluentValidation validators
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<EditUserDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<NoteDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<GroupDtoValidator>();

// Don`t forget to scope other classes in the program.cs

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "NotesApp v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
