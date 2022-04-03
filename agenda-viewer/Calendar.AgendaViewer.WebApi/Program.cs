using System;
using Calendar.Agenda.Domain.Entities.Messages;
using Calendar.AgendaViewer.DataAccess.Contracts;
using Calendar.AgendaViewer.DataAccess.Memory;
using Calendar.AgendaViewer.Domain;
using Calendar.Kafka.Configuration.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using StackExchange.Redis;

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
    builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(
        new ConfigurationOptions
        {
            EndPoints = {builder.Configuration["Redis:Persistent"]},
            Password = builder.Configuration["Redis:Password"]
        }));
    builder.Services.AddSingleton<IEventsRepository, EventsRepository>();

    builder.Services.Configure<KafkaConsumerOptions>(builder.Configuration.GetSection("Kafka"));
    builder.Services.AddKafkaConsumer<EventAddedMessageHandler, EventAddedMessage>();
    builder.Services.AddKafkaConsumer<EventRemovedMessageHandler, EventRemovedMessage>();

    builder.Services.AddHealthChecks();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

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