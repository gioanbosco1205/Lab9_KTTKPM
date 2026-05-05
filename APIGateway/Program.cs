using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add configuration.json
builder.Configuration.AddJsonFile("configuration.json", optional: false, reloadOnChange: true);

// Configure JWT Authentication
var audienceConfig = builder.Configuration.GetSection("Audience");
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(audienceConfig["Secret"]!));

var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = signingKey,
    ValidateIssuer = true,
    ValidIssuer = audienceConfig["Iss"],
    ValidateAudience = true,
    ValidAudience = audienceConfig["Aud"],
    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero,
    RequireExpirationTime = true,
};

// Add Authentication with custom scheme "TestKey"
builder.Services.AddAuthentication()
    .AddJwtBearer("TestKey", x =>
    {
        x.RequireHttpsMetadata = false;
        x.TokenValidationParameters = tokenValidationParameters;
    });

// Add Ocelot services
builder.Services.AddOcelot(builder.Configuration);

// Configure URL
builder.WebHost.UseUrls("http://localhost:9000");

var app = builder.Build();

// Use Ocelot middleware
await app.UseOcelot();

app.Run();
