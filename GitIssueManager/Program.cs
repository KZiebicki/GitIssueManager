using GitIssueManager.Core.Services;
using Microsoft.OpenApi.Models;
using GitIssueManager.Api;
using System.Text.Json.Serialization;
using System.Text.Json;
using GitIssueManager.Core.Factories.Interfaces;
using GitIssueManager.Core.Factories;

namespace GitIssueManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //TODO: Logs (Serilog)
            //TODO: Create documentation (README.md)

            builder.Services.AddControllers()
            .AddJsonOptions(o =>
             {
                 o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: true));
             });

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo{ Title = "Git Issue Manager API", Version = "v1" });
                c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Description = @"Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.OperationFilter<AuthenticationRequirementsOperationFilter>();
            });

            builder.Services.AddOpenApi();
            builder.Services.AddHttpClient();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<GitHubIssueService>();
            builder.Services.AddScoped<GitLabIssueService>();
            builder.Services.AddScoped<IIssueServiceFactory, IssueServiceFactory>();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
