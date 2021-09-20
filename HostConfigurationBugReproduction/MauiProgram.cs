using HostConfigurationBugReproduction.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;

namespace HostConfigurationBugReproduction
{
    public static class MauiProgram
    {
        private static void AddJsonFromResource(IConfigurationBuilder configurationBuilder, string fileName)
        {
            string resourceName = Assembly.GetExecutingAssembly().GetManifestResourceNames()
                        .Single(str => str.EndsWith(fileName));

            if (!string.IsNullOrWhiteSpace(resourceName))
            {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    stream.Seek(0, SeekOrigin.Begin);

                    MemoryStream reader = new MemoryStream();
                    stream.CopyTo(reader);
                    reader.Seek(0, SeekOrigin.Begin);

                    configurationBuilder.AddJsonStream(reader);
                }
            }
        }

        public static MauiApp CreateMauiApp()
        {

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                })

               .Host.ConfigureHostConfiguration(configurationBuilder =>
               {
                   AddJsonFromResource(configurationBuilder, "appsettings.json");
               })
#if DEBUG
                .ConfigureHostConfiguration(configurationBuilder =>
                {
                    AddJsonFromResource(configurationBuilder, "appsettings.Debug.json");
                })
#endif
#if RELEASE
                .ConfigureHostConfiguration(configurationBuilder =>
                {
                    AddJsonFromResource(configurationBuilder, "appsettings.json");
                })
#endif

                .ConfigureServices((context, services) =>
                {
                  services.Configure<AppOptions>(context.Configuration.GetRequiredSection("AppOptions"));
                });

            return builder.Build();
        }
    }
}