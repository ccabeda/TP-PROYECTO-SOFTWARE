using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Reflection;
using TP_PROYECTO_SOFTWARE.API.Middleware;
using TP_PROYECTO_SOFTWARE.API.Security;
using TP_PROYECTO_SOFTWARE.Aplication.Configuration;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.ICommand;
using TP_PROYECTO_SOFTWARE.Aplication.IRepository.IQuery;
using TP_PROYECTO_SOFTWARE.Aplication.ISecurity;
using TP_PROYECTO_SOFTWARE.Aplication.IUnitOfWork;
using TP_PROYECTO_SOFTWARE.Aplication.Mapping;
using TP_PROYECTO_SOFTWARE.Aplication.Services.Seats;
using TP_PROYECTO_SOFTWARE.Aplication.Validations.Event;
using TP_PROYECTO_SOFTWARE.Aplication.Validations.Seat;
using TP_PROYECTO_SOFTWARE.Aplication.Validations.Sector;
using TP_PROYECTO_SOFTWARE.Aplication.Validations.User;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.AuditLogs.Handlers;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Events.Handlers;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Reservations.Handlers;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Seats.Handlers;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Sectors.Handlers;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Users.Handlers;
using TP_PROYECTO_SOFTWARE.Domain.Models;
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
    //configurar swagger para autentication, si usas postman no influye. Configuración sacada de internet para logearse en la interfaz Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Ingresar: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key no configurado.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer no configurado.");
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience no configurado.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddDbContext<AplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));
});

builder.Services
    .AddIdentityCore<User>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 1;
    })
    .AddRoles<IdentityRole<int>>()
    .AddEntityFrameworkStores<AplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddOptions<TicketingRulesOptions>() //reglas dinamicas pidio profe
    .Bind(builder.Configuration.GetSection(TicketingRulesOptions.SectionName))
    .Validate(options => options.MaxSectorsPerEvent > 0, "MaxSectorsPerEvent debe ser mayor a 0.")
    .Validate(options => options.MaxSectorCapacity > 0, "MaxSectorCapacity debe ser mayor a 0.")
    .Validate(options => options.MaxRowsPerBulkCreate > 0, "MaxRowsPerBulkCreate debe ser mayor a 0.")
    .Validate(options => options.MaxSeatsPerRow > 0, "MaxSeatsPerRow debe ser mayor a 0.")
    .Validate(options => options.RowLabels.Count >= options.MaxRowsPerBulkCreate,
        "RowLabels debe tener al menos la misma cantidad de filas que MaxRowsPerBulkCreate.")
    .Validate(options => options.RowLabels
        .Take(options.MaxRowsPerBulkCreate)
        .Select(row => row.Trim().ToUpperInvariant())
        .Where(row => !string.IsNullOrWhiteSpace(row))
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .Count() == options.MaxRowsPerBulkCreate,
        "Las filas configuradas en RowLabels deben ser únicas y no vacías.")
    .ValidateOnStart();

builder.Services.AddOptions<AuthorizationSettingsOptions>()
    .Bind(builder.Configuration.GetSection(AuthorizationSettingsOptions.SectionName))
    .ValidateOnStart();

builder.Services.AddAutoMapper(_ => { }, typeof(AutomapperConfig).Assembly); //automapper
builder.Services.AddScoped<IRepositoryEventQuery, RepositoryEventQuery>();
builder.Services.AddScoped<IRepositoryAuditLogQuery, RepositoryAuditLogQuery>();
builder.Services.AddScoped<IRepositorySectorQuery, RepositorySectorQuery>();
builder.Services.AddScoped<IRepositorySeatQuery, RepositorySeatQuery>();
builder.Services.AddScoped<IRepositoryReservationQuery, RepositoryReservationQuery>();
builder.Services.AddScoped<IRepositoryUserQuery, RepositoryUserQuery>();
builder.Services.AddScoped<IRepositoryReservationCommand, RepositoryReservationCommand>();
builder.Services.AddScoped<IRepositorySeatCommand, RepositorySeatCommand>();
builder.Services.AddScoped<IRepositoryEventCommand, RepositoryEventCommand>();
builder.Services.AddScoped<IRepositorySectorCommand, RepositorySectorCommand>();
builder.Services.AddScoped<IRepositoryAuditLogCommand, RepositoryAuditLogCommand>();
builder.Services.AddScoped<IRepositoryUserCommand, RepositoryUserCommand>();
builder.Services.AddScoped<IUnitOfWorkReservationCommand, UnitOfWorkReservationCommand>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<ISeatRulesService, SeatRulesService>();
builder.Services.AddScoped<IGetEventsHandler, GetEventsHandler>();
builder.Services.AddScoped<IGetAuditLogsHandler, GetAuditLogsHandler>();
builder.Services.AddScoped<IGetEventByIdHandler, GetEventByIdHandler>();
builder.Services.AddScoped<ICreateEventHandler, CreateEventHandler>();
builder.Services.AddScoped<IDeleteEventHandler, DeleteEventHandler>();
builder.Services.AddScoped<IGetSectorsByEventHandler, GetSectorsByEventHandler>();
builder.Services.AddScoped<IGetSectorByIdHandler, GetSectorByIdHandler>();
builder.Services.AddScoped<ICreateSectorHandler, CreateSectorHandler>();
builder.Services.AddScoped<IDeleteSectorHandler, DeleteSectorHandler>();
builder.Services.AddScoped<IGetSeatsByEventHandler, GetSeatsByEventHandler>();
builder.Services.AddScoped<IGetSeatsBySectorHandler, GetSeatsBySectorHandler>();
builder.Services.AddScoped<IGetSeatByIdHandler, GetSeatByIdHandler>();
builder.Services.AddScoped<ICreateSeatHandler, CreateSeatHandler>();
builder.Services.AddScoped<ICreateSeatsBulkHandler, CreateSeatsBulkHandler>();
builder.Services.AddScoped<IDeleteSeatHandler, DeleteSeatHandler>();
builder.Services.AddScoped<IGetUsersHandler, GetUsersHandler>();
builder.Services.AddScoped<IGetCurrentUserHandler, GetCurrentUserHandler>();
builder.Services.AddScoped<IGetUserByIdHandler, GetUserByIdHandler>();
builder.Services.AddScoped<ICreateUserHandler, CreateUserHandler>();
builder.Services.AddScoped<ILoginUserHandler, LoginUserHandler>();
builder.Services.AddScoped<ICreateAuditLogHandler, CreateAuditLogHandler>();
builder.Services.AddScoped<ICreateReservationHandler, CreateReservationHandler>();
builder.Services.AddScoped<IConfirmReservationPaymentHandler, ConfirmReservationPaymentHandler>();
builder.Services.AddScoped<IGetReservationByIdHandler, GetReservationByIdHandler>();
builder.Services.AddValidatorsFromAssemblyContaining<UserCreateValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<EventCreateValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<SectorCreateValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<SeatCreateValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<SeatBulkCreateValidator>();
builder.Services.AddFluentValidationAutoValidation(); //validaciones

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    var adminEmails = builder.Configuration.GetSection("AuthorizationSettings:AdminEmails").Get<string[]>() ?? Array.Empty<string>();

    foreach (var role in new[] { "Admin", "User" })
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole<int>(role));
        }
    }

    foreach (var adminEmail in adminEmails)
    {
        var existingUser = await userManager.FindByEmailAsync(adminEmail);
        if (existingUser is null)
        {
            continue;
        }

        if (!await userManager.IsInRoleAsync(existingUser, "Admin"))
        {
            await userManager.AddToRoleAsync(existingUser, "Admin");
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>(); //middleware para manejar las excepciones de forma global y devolver respuestas consistentes

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
