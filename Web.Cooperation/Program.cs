using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Linq;
using Web.Cooperation;

var builder = WebApplication.CreateBuilder(args);

// Your Startup pattern
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

// Localization (do it once)
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

var supportedCultures = new[] { "en", "fr", "rw" }
    .Select(c => new CultureInfo(c))
    .ToList();

var app = builder.Build();

// RequestLocalization MUST be before routing/endpoints
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

// Seed before your pipeline is fine
await IdentitySeeder.SeedAsync(app.Services);

// Your Startup pipeline
startup.Configure(app, app.Environment);

app.Run();

