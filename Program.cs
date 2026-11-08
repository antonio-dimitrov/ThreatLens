using Microsoft.AspNetCore.Identity;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using ThreatLens.Api.Data;
using ThreatLens.Api.Services;
using ThreatLens.Api.Infrastructure;

namespace ThreatLens.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

           
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services
               .AddApplicationServices(builder.Configuration)
               .AddJwtAuthentication(builder.Configuration, builder.Environment)
               .AddErrorHandling();


            var app = builder.Build();

            app.UseExceptionHandler();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            //app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            await IdentitySeeder.SeedAdminAsync(app.Services);

            await app.RunAsync();
        }
    }
}
