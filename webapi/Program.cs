using Przychodnia.Webapi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Przychodnia.Webapi.Services;
using Przychodnia.Webapi.Models;
using Przychodnia.Shared;
using System.Text.Json.Serialization;
using Przychodnia.Webapi.CustomTokenProviders;
using Przychodnia.Webapi.Websocket;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "_myAllowSpecificOrigins",
        policy =>
        {
            policy.WithOrigins("http://localhost:8080").AllowAnyHeader().AllowAnyMethod();
        });
});

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("SqlServer") 
    ?? throw new InvalidOperationException("Connection string 'SqlServer' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedEmail = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityCore<Patient>(config =>
{
    config.Tokens.PasswordResetTokenProvider = nameof(PasswordResetTokenProvider<Patient>);
    config.Tokens.EmailConfirmationTokenProvider = nameof(EmailConfirmationTokenProvider<Patient>);
}).AddDefaultTokenProviders()
    .AddTokenProvider<PasswordResetTokenProvider<Patient>>(nameof(PasswordResetTokenProvider<Patient>))
    .AddTokenProvider<EmailConfirmationTokenProvider<Patient>>(nameof(EmailConfirmationTokenProvider<Patient>))
    .AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddIdentityCore<Employee>()
    .AddTokenProvider<PasswordResetTokenProvider<Employee>>(nameof(PasswordResetTokenProvider<Employee>))
    .AddDefaultTokenProviders().AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;
    options.Password.RequireNonAlphanumeric = false;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;

    options.SignIn.RequireConfirmedEmail = true;
});

builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["AuthSettings:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["AuthSettings:Audience"],
        RequireExpirationTime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AuthSettings:Key"] ?? "spare key" )),
        ValidateIssuerSigningKey = true,
    };
});

builder.Services.AddScoped<IUserService<RegisterDto, LoginDto>, PatientService>();
builder.Services.AddScoped<IUserService<RegisterEmployeeDto, LoginDto>, EmployeeService>();
builder.Services.AddScoped<PasswordResetTokenProvider<Patient>>();
builder.Services.AddScoped<PasswordResetTokenProvider<Employee>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/*app.UseHttpsRedirection();*/  // POTEM ODKOMENTOWAC

app.UseCors("_myAllowSpecificOrigins");

app.UseAuthentication();
app.UseAuthorization();

// configure websocket
var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(5)
};
webSocketOptions.AllowedOrigins.Add("http://localhost:8080");

app.UseWebSockets(webSocketOptions);
app.UseMiddleware<WebSocketMiddleware>();

app.MapControllers();

app.Run();
