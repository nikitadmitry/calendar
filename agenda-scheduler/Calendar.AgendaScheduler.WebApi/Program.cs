using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Calendar.AgendaScheduler.DataAccess.Interfaces;
using Calendar.AgendaScheduler.DataAccess.MongoDb;
using Calendar.AgendaScheduler.Domain;
using Calendar.AgendaScheduler.Domain.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var settings = MongoClientSettings.FromUrl(MongoUrl.Create(builder.Configuration["Mongo:Url"]));
    settings.ConnectTimeout = TimeSpan.FromSeconds(
        builder.Configuration.GetValue("Mongo:ConnectTimeoutSeconds", 5));

    var client = new MongoClient(settings);
    var database = client.GetDatabase(builder.Configuration["Mongo:DatabaseName"]);

    return database;
});
builder.Services.AddSingleton<IEventsRepository, EventsRepository>();
builder.Services.AddSingleton<IInitializable, EventsRepository>();
builder.Services.AddSingleton<IEventValidator, EventValidator>();
builder.Services.AddSingleton<IEventScheduler, EventScheduler>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.MapControllers();

app.Run();