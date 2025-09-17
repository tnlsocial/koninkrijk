using koninkrijk.Server.Data;
using koninkrijk.Server.Helpers;
using koninkrijk.Server.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Cryptography;

namespace koninkrijk.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<DataContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DataContext") ?? throw new InvalidOperationException("Connection string 'DataContext' not found.")));

            builder.Services.AddAuthorization();
            builder.Services.AddControllers(); 

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "MyAllowSpecificOrigins", builder =>
                {
                    builder
                        .WithOrigins("https://koninkrijk.test.example", "http://koninkrijk.test.example")
                        .WithHeaders("Authorization", "Content-Type")
                        .WithMethods("GET", "POST", "OPTIONS", "PUT", "DELETE");
                });
            });

            builder.Services.AddAuthentication("ApiKeyScheme").AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("ApiKeyScheme", options => { });
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "koninkrijk", Version = "v1" });
                // Add security definition for API key
                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "API key needed to access the endpoints",
                    Scheme = "ApiKey"
                });

                // Apply security requirements to endpoints
                c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "ApiKey"
                            }
                        },
                        new string[] {}
                        }
                    });
            });


            builder.Services.AddRouting(options => options.LowercaseUrls = true);

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseCors("MyAllowSpecificOrigins");
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                app.UseCors(x => x
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .SetIsOriginAllowed(origin => true) // allow any origin
                   .AllowCredentials()); // allow credentials
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
