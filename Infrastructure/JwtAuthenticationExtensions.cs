using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ThreatLens.Api.Services;

namespace ThreatLens.Api.Infrastructure
{
    public static class JwtAuthenticationExtensions
    {

        public static IServiceCollection AddJwtAuthentication(
           this IServiceCollection services,
           IConfiguration configuration,
           IHostEnvironment environment)
        {
            // Bind + validate the "Jwt" section; the app refuses to start if it is invalid
            services.AddOptions<JwtSettings>()
                .Bind(configuration.GetSection(JwtSettings.SectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            var jwt = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
                ?? throw new InvalidOperationException("Jwt section is missing.");

            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwt.Issuer,

                        ValidateAudience = true,
                        ValidAudience = jwt.Audience,

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromSeconds(30),   // default is 5 minutes of extra life

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwt.SecretKey))
                    };

                    // Debug helpers, Development only
                    if (environment.IsDevelopment())
                    {
                        options.Events = new JwtBearerEvents
                        {
                            OnAuthenticationFailed = context =>
                            {
                                context.Response.Headers["X-JWT-Error"] =
                                    context.Exception.GetType().Name;
                                return Task.CompletedTask;
                            },

                            OnChallenge = context =>
                            {
                                var logger = context.HttpContext.RequestServices
                                    .GetRequiredService<ILogger<Program>>();

                                logger.LogWarning(
                                    "JWT challenge: {Error} - {Description}",
                                    context.Error,
                                    context.ErrorDescription);

                                return Task.CompletedTask;
                            }
                        };
                    }
                });

            services.AddAuthorization();

            return services;
        }
    }
}
