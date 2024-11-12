using AdminPanel.DataModel.Context;
using AdminPanel.Services;
using AdminPanel.Services.Helpers;
using AdminPanel.Services.Repository;
using AdminPanel.Services.Repository.Account;
using AdminPanel.Services.Repository.MasterAdmin;
using AdminPanel.Services.ServicesLayer;
using AdminPanel.Services.ServicesLayer.CompanyAdmin;
using AdminPanel.Shared;
using AdminPanel.Shared.Common;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Stripe;
using ProductService = AdminPanel.Services.ServicesLayer.CompanyAdmin.ProductService;
using CustomerService = AdminPanel.Services.ServicesLayer.CustomerService;
using DiscountService = AdminPanel.Services.ServicesLayer.DiscountService;
using EZOrders.PaymentGateway.Stripe;
using NLog;
using AdminPanel.Server;
using EZOrders.PaymentGateway.Razorpay;

var builder = WebApplication.CreateBuilder(args);

// Add dbcontext to the project
var connectionString = builder.Configuration.GetConnectionString("OrderflowConnString");
builder.Services.AddDbContext<OrderflowContext>(options => options.UseSqlServer(connectionString));


// define authentication scheme
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options => options.LoginPath = new PathString("/Login"));

builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Email settings
builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection("EmailSettings"));

// send emails class
builder.Services.AddScoped<IMailService, MailService>();

// commpn application settings
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<PaymentSettings>(builder.Configuration.GetSection("Payments"));

// Set stripe key
string? secret = builder.Configuration.GetValue<string>("Payments:StripeSecret");
StripeConfiguration.ApiKey = secret;

// created extention to add DIs for Repo and service layers
builder.Services.AddServiceRepos();

builder.Services.AddScoped<IStripePaymentProcessor, StripePaymentProcessor>();
builder.Services.AddScoped<IRazorpayPaymentProcessor, RazorpayPaymentProcessor>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
