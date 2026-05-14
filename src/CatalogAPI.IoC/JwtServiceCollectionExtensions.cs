using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CatalogAPI.IoC;

public static class JwtServiceCollectionExtensions
{
    public static IServiceCollection AddJwtAuthenticationConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var secret = configuration["Jwt:Key"];
        if (string.IsNullOrWhiteSpace(secret))
        {
            throw new ArgumentNullException("Jwt:Key", "A chave JWT (Jwt:Key) não pode ser nula ou vazia.");
        }

        var issuer = configuration["Jwt:Issuer"];
        if (string.IsNullOrWhiteSpace(issuer))
        {
            throw new ArgumentNullException("Jwt:Issuer", "O emissor JWT (Jwt:Issuer) não pode ser nulo ou vazio.");
        }

        var audience = configuration["Jwt:Audience"];
        if (string.IsNullOrWhiteSpace(audience))
        {
            throw new ArgumentNullException("Jwt:Audience", "A audiência JWT (Jwt:Audience) não pode ser nula ou vazia.");
        }

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(secret)),
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = ctx =>
                    {
                        var logger = ctx.HttpContext.RequestServices
                            .GetRequiredService<ILogger<JwtBearerEvents>>();
                        logger.LogWarning("[JWT] OnMessageReceived | Authorization header presente={HasHeader} | Token extraído={HasToken}",
                            ctx.Request.Headers.ContainsKey("Authorization"),
                            !string.IsNullOrEmpty(ctx.Token));
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = ctx =>
                    {
                        var logger = ctx.HttpContext.RequestServices
                            .GetRequiredService<ILogger<JwtBearerEvents>>();
                        logger.LogWarning("[JWT] OnAuthenticationFailed: {Error} | ValidIssuer={Issuer} | ValidAudience={Audience}",
                            ctx.Exception.Message, issuer, audience);
                        return Task.CompletedTask;
                    },
                    OnChallenge = ctx =>
                    {
                        var logger = ctx.HttpContext.RequestServices
                            .GetRequiredService<ILogger<JwtBearerEvents>>();
                        logger.LogWarning("[JWT] OnChallenge (401 emitido): Error={Error} | ErrorDescription={Description}",
                            ctx.Error, ctx.ErrorDescription);
                        return Task.CompletedTask;
                    }
                };
            });
        services.AddAuthorization();
        return services;
    }
}