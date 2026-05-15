using CatalogAPI.Api.Endpoints;
using CatalogAPI.Api.Middleware;
using CatalogAPI.Data;
using CatalogAPI.IoC;
using CatalogAPI.Messaging;
using Microsoft.EntityFrameworkCore;
using Prometheus;
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
builder.Services.AddRabbitMQMessaging(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

// Inicializar RabbitMQ de forma ass�ncrona
try
{
    using var scope = app.Services.CreateScope();
    var initializer = scope.ServiceProvider.GetRequiredService<RabbitMQInitializer>();
    await initializer.InitializeAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Erro ao inicializar RabbitMQ");
    throw;
}


//rodar migrations Auto.
try
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<CatalogApiDbContext>();
    db.Database.Migrate();

    Log.Information("Migrations aplicadas com sucesso.");
}
catch(Exception ex)
{
    Log.Information(ex, "Erro ao aplicar migrations");
    throw;
}

app.UseMiddleware<LoggingMiddleware>();
app.UseSerilogRequestLoggingConfiguration();
app.UseHttpsRedirection();
app.UseHttpMetrics();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapMetrics();
app.MapControllers();
app.MapGames();
app.MapUsuarioGameBiblioteca();

app.Run();