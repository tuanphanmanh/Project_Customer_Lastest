using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using esign.Web.Helpers;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace esign.Web.Startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var defaultApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(Path.Combine(Directory.GetCurrentDirectory(), "FirebaseKey.json")),
            });
            CurrentDirectoryHelpers.SetCurrentDirectory();
            CreateWebHostBuilder(args).Build().Run();

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return new WebHostBuilder()
                .UseKestrel(opt =>
                {
                    opt.AddServerHeader = false;
                    opt.Limits.MaxRequestLineSize = 16 * 1024;
                    opt.Limits.MaxRequestBodySize = null;
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureLogging((context, logging) =>
                {
                    logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
                })
                .UseIIS()
                .UseIISIntegration()
                .UseStartup<Startup>();
        }
    }
}
