using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add configuration.json
builder.Configuration.AddJsonFile("configuration.json", optional: false, reloadOnChange: true);

// Add Ocelot services
builder.Services.AddOcelot(builder.Configuration);

// Configure URL
builder.WebHost.UseUrls("http://localhost:9000");

var app = builder.Build();

// Use Ocelot middleware
await app.UseOcelot();

app.Run();
