using Microsoft.AspNetCore.Identity;
using TP_PROYECTO_SOFTWARE.API;
using TP_PROYECTO_SOFTWARE.API.Middleware;
using TP_PROYECTO_SOFTWARE.Aplication;
using TP_PROYECTO_SOFTWARE.Domain.Models;
using TP_PROYECTO_SOFTWARE.Infraestructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

await app.InitializeAuthorizationAsync(builder.Configuration);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>(); //middleware para manejar las excepciones de forma global y devolver respuestas consistentes

app.UseHttpsRedirection();

app.UseCors("FrontendDevPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
