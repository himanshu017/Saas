using AdminPanel.Client;
using AdminPanel.Client.AuthProviders;
using AdminPanel.Client.Services;
using AdminPanel.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();

// Enabling authentication 
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<CustomAuthenticationStateProvider>());
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IStateService, StateService>();
builder.Services.AddScoped<IHttpService, HttpService>();
builder.Services.AddScoped<INotify, Notify>();
builder.Services.AddScoped<LoadingService>();

await builder.Build().RunAsync();
