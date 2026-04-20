using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Reflection;
using TP_PROYECTO_SOFTWARE.API.Middleware;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.IUnitOfWork;
using TP_PROYECTO_SOFTWARE.Aplication.Mapping;
using TP_PROYECTO_SOFTWARE.Aplication.Validations.Reservation;
using TP_PROYECTO_SOFTWARE.Aplication.Validations.User;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.AuditLogs.Handlers;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Events.Handlers;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Reservations.Handlers;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Handlers;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Sectors.Handlers;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Users.Handlers;
using TP_PROYECTO_SOFTWARE.Infraestructure.Persistence;
using TP_PROYECTO_SOFTWARE.Infraestructure.Repository.Command;
using TP_PROYECTO_SOFTWARE.Infraestructure.Repository.Query;
using TP_PROYECTO_SOFTWARE.Infraestructure.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => //simplemente decoracion para que se vea mejor swagger y para que tome los comentarios xml de los controladores
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sistema de Ticketing",
        Version = "v1",
        Description = "API desarrollada para la gestión de eventos, asientos y reservas."
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddDbContext<AplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));
});

builder.Services.AddAutoMapper(_ => { }, typeof(AutomapperConfig).Assembly); //automapper
builder.Services.AddScoped<IRepositoryEventQuery, RepositoryEventQuery>();
builder.Services.AddScoped<IRepositorySectorQuery, RepositorySectorQuery>();
builder.Services.AddScoped<IRepositorySeatQuery, RepositorySeatQuery>();
builder.Services.AddScoped<IRepositoryReservationQuery, RepositoryReservationQuery>();
builder.Services.AddScoped<IRepositoryUserQuery, RepositoryUserQuery>();
builder.Services.AddScoped<IRepositoryReservationCommand, RepositoryReservationCommand>();
builder.Services.AddScoped<IRepositorySeatCommand, RepositorySeatCommand>();
builder.Services.AddScoped<IRepositoryAuditLogCommand, RepositoryAuditLogCommand>();
builder.Services.AddScoped<IRepositoryUserCommand, RepositoryUserCommand>();
builder.Services.AddScoped<IUnitOfWorkReservationCommand, UnitOfWorkReservationCommand>();
builder.Services.AddScoped<IGetEventsHandler, GetEventsHandler>();
builder.Services.AddScoped<IGetSectorsByEventHandler, GetSectorsByEventHandler>();
builder.Services.AddScoped<IGetSeatsByEventHandler, GetSeatsByEventHandler>();
builder.Services.AddScoped<IGetSeatsBySectorHandler, GetSeatsBySectorHandler>();
builder.Services.AddScoped<IGetUsersHandler, GetUsersHandler>();
builder.Services.AddScoped<IGetUserByIdHandler, GetUserByIdHandler>();
builder.Services.AddScoped<ICreateUserHandler, CreateUserHandler>();
builder.Services.AddScoped<ILoginUserHandler, LoginUserHandler>();
builder.Services.AddScoped<ICreateAuditLogHandler, CreateAuditLogHandler>();
builder.Services.AddScoped<ICreateReservationHandler, CreateReservationHandler>();
builder.Services.AddScoped<IGetReservationByIdHandler, GetReservationByIdHandler>();
builder.Services.AddValidatorsFromAssemblyContaining<UserCreateValidator>();
builder.Services.AddFluentValidationAutoValidation(); //validaciones

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>(); //middleware para manejar las excepciones de forma global y devolver respuestas consistentes

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
