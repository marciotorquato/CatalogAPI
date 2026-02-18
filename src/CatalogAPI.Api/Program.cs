using CatalogAPI.Api.Endpoints;
using CatalogAPI.Api.Middleware;
using CatalogAPI.Data;
using CatalogAPI.IoC;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJwtAuthenticationConfig(builder.Configuration);
builder.Services.AddSwaggerDocumentation();
builder.Services.AddControllers();
builder.AddSerilogConfiguration();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<CatalogApiDbContext>(options => options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("MS_CatalogAPI")));
builder.Services.AddApplicationServices();
builder.Services.AddDomainServices();
builder.Services.AddRepositories();
builder.Host.UseSerilog();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

app.UseMiddleware<LoggingMiddleware>();
app.UseSerilogRequestLoggingConfiguration();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();
app.MapGames();
app.MapUsuarioGameBiblioteca();

app.Run();