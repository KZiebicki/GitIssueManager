using GitIssueManager.Core.Services.Interfaces;
using GitIssueManager.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace GitIssueManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen(c =>
            {
                //WIP: MAKE IT WORK WITH AUTHORIZATION
                c.SwaggerDoc("v1", new OpenApiInfo{ Title = "Git Issue Manager API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
            });


            builder.Services.AddOpenApi();
            builder.Services.AddHttpClient();

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
