using Catalog.Repositories;
using Catalog.Settings;
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

        MongoDBSettings MongoDBSettings = builder.Configuration.GetSection(nameof(MongoDBSettings)).Get<MongoDBSettings>();

        // Add services to the container.
        builder.Services.AddSingleton<IMongoClient>((serviceProvider) =>
        {
            return new MongoClient(MongoDBSettings.ConnectionString);
        });
        builder.Services.AddSingleton<IItemsRepository, MongoDBItemsRepository>();

        // Tell the controllers to not suppress the Async suffix from function names.
        builder.Services.AddControllers((options) =>
        {
            options.SuppressAsyncSuffixInActionNames = false;
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddHealthChecks().AddMongoDb(MongoDBSettings.ConnectionString, name: "monogodb", timeout: TimeSpan.FromSeconds(3));

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
        app.MapHealthChecks("/api/health");

        app.Run();
    }
}