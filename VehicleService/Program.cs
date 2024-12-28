using Microsoft.IdentityModel.Tokens;
using VehicleServiceAPI.Interfaces;
using VehicleServiceAPI.Services;
using System.Text;
using VehicleServiceAPI.JWT;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IVehicleService, VehicleService>();

// Use environment variables or config for sensitive values
var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? "0123456789abcdef0123456789abcdef";  // Fallback to a default if not found
var issuer = "admin";  
var audience = "user";  

builder.Services.AddScoped<JwtTokenService>(provider => new JwtTokenService(secretKey, issuer, audience));

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            RoleClaimType = "role" 
        };
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();



app.Use(async (context, next) =>
{
    var user = context.User;
    if (user.Identity.IsAuthenticated)
    {
        Console.WriteLine($"Authenticated User: {user.Identity.Name}");
        var role = user.FindFirst("role")?.Value;
        Console.WriteLine($"User Role: {role}");
    }
    else
    {
        Console.WriteLine("Unauthenticated User");
    }

    await next();
});


app.Run();

