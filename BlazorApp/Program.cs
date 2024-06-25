using BlazorApp;
using BlazorApp.Components;
using BlazorApp.Services;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using MudBlazor.Services;

const string _connectionString = "DataSource=TBS.db;Cache=Shared";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options
#if DEBUG
            .EnableSensitiveDataLogging()
#endif
            .UseSqlite(_connectionString)
);

builder.Services.AddMudServices(c => { c.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopLeft; });
builder.Services.AddSignalR(options =>
{
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(4);
    options.KeepAliveInterval = TimeSpan.FromSeconds(2);
});
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddHostedService<StartUpService>();
builder.Services.AddSingleton<RoomManager>();
builder.Services.AddScoped(sp =>
{
    var navMan = sp.GetRequiredService<NavigationManager>();
    return new HubConnectionBuilder()
        .WithUrl(navMan.ToAbsoluteUri("/noituhub"))
        .WithAutomaticReconnect()
        .Build();
});
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();
app.MapHub<NoiTuHub>("/noituhub");

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await app.RunAsync();