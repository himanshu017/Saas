using AdminPanel.API.AuthTokenService;
using AdminPanel.DataModel.Context;
using AdminPanel.Services.Helpers;
using AdminPanel.Services.Repository;
using AdminPanel.Services.Repository.Account;
using AdminPanel.Services.ServicesLayer;
using AdminPanel.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add dbcontext to the project
var connectionString = builder.Configuration.GetConnectionString("OrderflowConnString");
builder.Services.AddDbContext<OrderflowContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Email settings
builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection("EmailSettings"));

// commpn application settings
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// send emails class
builder.Services.AddScoped<IMailService, MailService>();

#region Swagger and Jwt initialization

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AdminPanel.API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."

    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         new string[] {}
                    }
                });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var key = builder.Configuration.GetValue<string>("AppSettings:Secret");
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key))
    };
});

builder.Services.AddSingleton<ITokenAuth>(new TokenAuth(key));

#endregion

// Add repository DI's
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<IProductsService, ProductsService>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICommonService, CommonService>();

LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

var app = builder.Build();

//// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); //figure out how to use this
}

app.UseSwagger();
app.UseSwaggerUI(o =>
{
    o.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    o.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
    app.MapControllers().WithMetadata(new AllowAnonymousAttribute());
else
    app.MapControllers();

app.Run();
