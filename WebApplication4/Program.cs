using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NotesApp.Application.Services;
using NotesApp.Domain.Interfaces;
using NotesApp.Infrastructure.Data;
using NotesApp.Infrastructure.Repositories;
using FluentValidation;
using NotesApp.Application.Validators;
using Presentation.Middlewares;
using Presentation.Requests.Users;
using Presentation.Requests.Notes;
using Presentation.Requests.Groups;
using Application.Interfaces;
using NotesApp.Domain.Entities;
using Application.Services;
using Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer()
    .AddSwaggerGen(c =>
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
    o.UseNpgsql(DotNetEnv.Env.GetString("CONNECTION_STRING_DB", "Server=127.0.0.1;Port=5432;Database=NotesApp;User id=postgres;Password=root"),
        o => o.MigrationsAssembly("Presentation")));

// Register Services, Interfaces, Repositories and Validators
builder.Services
    .AddScoped<IUserService, UserService>()
    .AddScoped<User.UserBuilder>()
    .AddScoped<IUserRepository, UserRepository>()
    .AddScoped<INoteService, NoteService>()
    .AddScoped<Note.NoteBuilder>()
    .AddScoped<INoteRepository, NoteRepository>()
    .AddScoped<IGroupService, GroupService>()
    .AddScoped<Group.GroupBuilder>()
    .AddScoped<IGroupRepository, GroupRepository>()
    .AddValidatorsFromAssemblyContaining<CreateUserRequestValidator>()
    .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    typeof(Program).Assembly, // Escaneia o projeto Presentation
    typeof(UserService).Assembly,
    typeof(UserRepository).Assembly// Escaneia o projeto Application
));


var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapUserEndpoints()
    .MapNoteEndpoints()
    .MapGroupEndpoints();


// Register the Middlewere

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

app.Run();
