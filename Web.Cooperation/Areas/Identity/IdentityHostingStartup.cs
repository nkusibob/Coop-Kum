using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Web.Cooperation.Areas.Identity.IdentityHostingStartup))]

namespace Web.Cooperation.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}