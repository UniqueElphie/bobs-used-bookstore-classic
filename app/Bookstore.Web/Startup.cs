using Microsoft.AspNetCore.Owin;
using Microsoft.Owin;
using Owin;
using NLog;


[assembly: OwinStartup(typeof(Bookstore.Web.Startup))]

namespace Bookstore.Web
{
    public static class AuthenticationConfig
    {
        public static void ConfigureAuthentication(IAppBuilder app)
        {
            // Authentication configuration code goes here
        }
    }

    public static class LoggingSetup
    {
        public static void ConfigureLogging()
        {
// Configure logging using NLog
            LogManager.Setup().LoadConfiguration(builder => {
                builder.ForLogger().FilterMinLevel(NLog.LogLevel.Info).WriteToConsole();
            });
        }
    }

    public static class ConfigurationSetup
    {
        public static void ConfigureConfiguration()
        {
            // Configuration code goes here
        }
    }

    public static class DependencyInjectionSetup
    {
        public static void ConfigureDependencyInjection(IAppBuilder app)
        {
            // Dependency injection configuration code goes here
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            LoggingSetup.ConfigureLogging();

            ConfigurationSetup.ConfigureConfiguration();

            DependencyInjectionSetup.ConfigureDependencyInjection(app);

            AuthenticationConfig.ConfigureAuthentication(app);
        }
    }
}