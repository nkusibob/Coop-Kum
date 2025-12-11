using Microsoft.AspNetCore.Builder;
using Web.Cooperation;

var builder = WebApplication.CreateBuilder(args);

// Create Startup instance
var startup = new Startup(builder.Configuration);

// Register services from Startup
startup.ConfigureServices(builder.Services);

var app = builder.Build();

// 🔥 Run Identity seeding BEFORE middleware
await IdentitySeeder.SeedAsync(app.Services);

// Configure middleware pipeline from Startup
startup.Configure(app, app.Environment);

app.Run();
