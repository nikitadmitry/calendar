using System;
using System.Linq;
using Calendar.Agenda.Domain.Entities.Messages;
using Calendar.AgendaScheduler.Client;
using Calendar.AgendaViewer.Client;
using Calendar.Client.Configuration.Extensions;
using Calendar.Kafka.Configuration.Extensions;
using Calendar.WebApp.Domain;
using Calendar.WebApp.Messaging;
using Calendar.WebApp.Messaging.Handlers;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Radzen;
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
    builder.Services.AddHealthChecks();
    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor();
    builder.Services.AddResponseCompression(opts =>
    {
        opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
    });
    builder.Services.AddScoped<DialogService>();
    builder.Services.AddScoped<NotificationService>();
    builder.Services.AddScoped<TooltipService>();
    builder.Services.AddScoped<ContextMenuService>();

    builder.Services.AddMediatR(typeof(EventsRequestHandler));

    builder.Services.Configure<ClientOptions<IAgendaViewerClient>>(builder.Configuration.GetSection("AgendaViewer"));
    builder.Services.AddClient<IAgendaViewerClient>();

    builder.Services.Configure<ClientOptions<IAgendaSchedulerClient>>(builder.Configuration.GetSection("AgendaScheduler"));
    builder.Services.AddClient<IAgendaSchedulerClient>();

    builder.Services.Configure<KafkaConsumerOptions>(builder.Configuration.GetSection("Kafka"));
    builder.Services.AddKafkaConsumer<EventAddedMessageHandler, EventAddedMessage>();
    builder.Services.AddKafkaConsumer<EventRemovedMessageHandler, EventRemovedMessage>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseStaticFiles();

    app.UseRouting();

    app.MapBlazorHub();
    app.MapHub<AgendaUpdatesHub>(Constants.AgendaUpdatesHub);
    app.MapFallbackToPage("/_Host");
    app.UseHealthChecks("/health");

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