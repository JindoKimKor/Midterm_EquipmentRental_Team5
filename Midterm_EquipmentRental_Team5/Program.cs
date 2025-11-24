using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Midterm_EquipmentRental_Team5.Infrastructure.Persistence;
using Midterm_EquipmentRental_Team5.Domain.Entities;
using Midterm_EquipmentRental_Team5.Infrastructure.Repositories;
using Midterm_EquipmentRental_Team5.Infrastructure.Repositories.Interfaces;
using Midterm_EquipmentRental_Team5.Application.Services;
using Midterm_EquipmentRental_Team5.Application.Interfaces;
using Midterm_EquipmentRental_Team5.Infrastructure.UnitOfWork;
using Midterm_EquipmentRental_Team5.Infrastructure;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Midterm_EquipmentRental_Team5.Presentation.Hubs;
using Midterm_EquipmentRental_Team5.Application.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Midterm_EquipmentRental_Team5.Application.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

// Jwt setting binding and Singleton lifecycle injection - Externalized Configuration 
var jwtSettings = new JwtSettings();

builder.Configuration.GetSection("JwtSettings").Bind(jwtSettings);
builder.Services.AddSingleton(jwtSettings);

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();
builder.Services.AddScoped<IRentalRepository, RentalRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();

builder.Services.AddScoped<ICustomerServices, CustomerServices>();
builder.Services.AddScoped<IEquipmentServices, EquipmentService>();
builder.Services.AddScoped<IRentalServices, RentalServices>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IChatService, ChatService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IClaimsTransformation, RoleClaimsTransformer>();

builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

builder.Services.AddHostedService<ClearMessage>();


// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend",
        policy =>
        {
            policy.WithOrigins(["http://localhost:5115", "https://localhost:5115", "http://localhost:5173"])
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

// DI injection - Database - InMemory Database - infrastructure layer
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("EquipmentRentalDb"));

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings.SecretKey)
        ),
        RoleClaimType = ClaimTypes.Role,
        NameClaimType = ClaimTypes.NameIdentifier
    };
})
.AddCookie(options =>
{
    options.LoginPath = "/api/auth/google-login";
    options.LogoutPath = "/api/auth/logout";
    options.AccessDeniedPath = "/api/auth/denied";
})
.AddGoogle(GoogleDefaults.AuthenticationScheme, o =>
{
    o.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
    o.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;

    o.Scope.Add("email");
    o.Scope.Add("profile");
});

// Authorization - Security Layer
builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSignalR();

builder.Services.AddSwaggerGen(options =>
{
    // No API versioning for this project
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Equipment Rental API",
        Version = "v1"
    });

    // JWT Bearer authentication in Swagger UI
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = JwtBearerDefaults.AuthenticationScheme.ToLower()
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            new string[] {}        }
    });
});

var app = builder.Build();
app.UseStaticFiles(); // Serves files from wwwroot

// Enable CORS
app.UseCors("Frontend");

// database intialized
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

// Swagger UI ( Development Environment )
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS Redirection
app.UseHttpsRedirection();

// Authentication
app.UseAuthentication();

// Authorization
app.UseAuthorization();

// MapControllers
app.MapControllers();

app.MapHub<ChatHub>("/chathub");

app.Run();
