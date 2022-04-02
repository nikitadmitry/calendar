using Calendar.Agenda.Domain.Entities.Messages;
using Calendar.AgendaViewer.DataAccess.Contracts;
using Calendar.AgendaViewer.DataAccess.Memory;
using Calendar.AgendaViewer.Domain;
using Calendar.Kafka.Configuration.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(
    builder.Configuration["Redis:Persistent"]));
builder.Services.AddSingleton<IEventsRepository, EventsRepository>();

builder.Services.Configure<KafkaConsumerOptions>(builder.Configuration.GetSection("Kafka"));
builder.Services.AddKafkaConsumer<EventAddedMessageHandler, EventAddedMessage>();
builder.Services.AddKafkaConsumer<EventRemovedMessageHandler, EventRemovedMessage>();

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

app.MapControllers();

app.Run();