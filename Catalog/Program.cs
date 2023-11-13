using System;
using System.Net.Mime;
using System.Text.Json;
using Catalog.Repositories;
using Catalog.Settings;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

internal class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Configure serializers so the types are correctly translated to strings.
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

        MongoDbSettings MongoDBSettings = builder.Configuration.GetSection(nameof(MongoDBSettings)).Get<MongoDbSettings>();

        // Add services to the container.
        builder.Services.AddSingleton<IMongoClient>((serviceProvider) =>
        {
            return new MongoClient(MongoDBSettings.ConnectionString);
        });
        builder.Services.AddSingleton<IItemsRepository, MongoDbItemsRepository>();

        // Tell the controllers to not suppress the Async suffix from function names.
        builder.Services.AddControllers((options) =>
        {
            options.SuppressAsyncSuffixInActionNames = false;
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Add health checks to the api. Also add a health check for the connection to the db.
        builder.Services.AddHealthChecks()
            .AddMongoDb(
                MongoDBSettings.ConnectionString,
                name: "monogodb",
                timeout:
                TimeSpan.FromSeconds(3),
                tags: new string[] { "ready" });

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.MapHealthChecks("/api/health/ready", new HealthCheckOptions()
        {
            Predicate = (check) => check.Tags.Contains("ready"),
            ResponseWriter = async (context, report) =>
            {
                string result = JsonSerializer.Serialize(new
                {
                    status = report.Status.ToString(),
                    checks = report.Entries.Select((entry) => new
                    {
                        name = entry.Key,
                        status = entry.Value.Status.ToString(),
                        exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
                        duration = entry.Value.Duration.ToString()
                    })
                });

                context.Response.ContentType = MediaTypeNames.Application.Json;
                await context.Response.WriteAsync(result);
            }
        });
        app.MapHealthChecks("/api/health/live", new HealthCheckOptions()
        {
            Predicate = (_) => false
        });

        app.Run();
    }
}