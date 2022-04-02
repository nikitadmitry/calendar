using Calendar.AgendaScheduler.Client;
using Calendar.AgendaViewer.Client;
using Calendar.Client.Configuration.Extensions;
using Calendar.WebApp.Domain;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

builder.Services.AddMediatR(typeof(EventsRequestHandler));

builder.Services.Configure<ClientOptions<IAgendaViewerClient>>(builder.Configuration.GetSection("AgendaViewer"));
builder.Services.AddClient<IAgendaViewerClient>();

builder.Services.Configure<ClientOptions<IAgendaSchedulerClient>>(builder.Configuration.GetSection("AgendaScheduler"));
builder.Services.AddClient<IAgendaSchedulerClient>();

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
app.MapFallbackToPage("/_Host");

app.Run();