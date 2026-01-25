using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Web.Cooperation;

namespace Web.Cooperation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

// Create Startup instance
var startup = new Startup(builder.Configuration);

// Register services from Startup
startup.ConfigureServices(builder.Services);
builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});
builder.Services
    .AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();
var supportedCultures = new[] { "en", "fr", "rw" };

var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture("en")
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);



var app = builder.Build();
app.UseRequestLocalization(localizationOptions);
// 🔥 Run Identity seeding BEFORE middleware
await IdentitySeeder.SeedAsync(app.Services);

// Configure middleware pipeline from Startup
startup.Configure(app, app.Environment);

app.Run();
