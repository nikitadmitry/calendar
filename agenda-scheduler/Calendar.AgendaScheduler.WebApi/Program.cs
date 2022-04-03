using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Calendar.AgendaScheduler.DataAccess.Interfaces;
using Calendar.AgendaScheduler.DataAccess.Kafka;
using Calendar.AgendaScheduler.DataAccess.MongoDb;
using Calendar.AgendaScheduler.Domain;
using Calendar.AgendaScheduler.Domain.Interfaces;
using Calendar.AgendaScheduler.WebApi.Configuration;
using Confluent.Kafka;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, sp, lc) => lc
        .WriteTo.Console()
        .ReadFrom.Configuration(ctx.Configuration));

    // Add services to the container.
    builder.Services.Configure<MongoConfig>(builder.Configuration.GetSection("Mongo"));
    builder.Services.AddSingleton<IMongoDatabase>(sp =>
    {
        var options = sp.GetRequiredService<IOptions<MongoConfig>>().Value;
        var settings = MongoClientSettings.FromUrl(MongoUrl.Create(options.Url));
        settings.ConnectTimeout = TimeSpan.FromSeconds(options.ConnectTimeoutSeconds);
        if (!(options.Credential?.Anonymous ?? true))
        {
            settings.Credential = MongoCredential.CreateCredential(
                options.Credential.Database,
                options.Credential.User,
                options.Credential.Password);
        }

        var client = new MongoClient(settings);
        var database = client.GetDatabase(options.DatabaseName);

        return database;
    });

    builder.Services.AddAutoMapper(cfg => cfg.AddProfile(typeof(MappingProfile)));
    builder.Services.AddSingleton<IProducer<Null, string>>(sp =>
    {
        var config = new ProducerConfig
        {
            BootstrapServers = builder.Configuration["Kafka:BrokerList"],
            ClientId = Dns.GetHostName()
        };

        var producer = new ProducerBuilder<Null, string>(config).Build();
        return producer;
    });
    builder.Services.AddSingleton<KafkaTopicsProvider>();
    builder.Services.AddSingleton<IMessagePublisher, KafkaPublisher>();

    builder.Services.AddSingleton<IEventsRepository, EventsRepository>();
    builder.Services.AddSingleton<IInitializable, EventsRepository>();
    builder.Services.AddSingleton<IEventValidator, EventValidator>();
    builder.Services.AddSingleton<IEventScheduler, EventScheduler>();
    builder.Services.AddSingleton<IEventRemover, EventRemover>();

    builder.Services.AddHealthChecks();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    var initializables = app.Services.GetServices<IInitializable>();
    var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));
    using (tokenSource)
    {
        await Task.WhenAll(initializables.Select(x => x.InitializeAsync(tokenSource.Token)));
    }

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();
    app.UseHealthChecks("/health");
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}