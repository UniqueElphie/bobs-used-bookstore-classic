using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bookstore.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureLogging(services);
            ConfigureAppConfiguration();
            ConfigureDependencyInjection(services);
            ConfigureAuthentication(services);
        }

        private void ConfigureLogging(IServiceCollection services)
        {
            // TODO: Implement logging configuration
            services.AddLogging(builder =>
            {
                builder.AddConfiguration(Configuration.GetSection("Logging"));
                builder.AddConsole();
            });
        }

        private void ConfigureAppConfiguration()
        {
            // TODO: Implement additional configuration setup if needed
        }

        private void ConfigureDependencyInjection(IServiceCollection services)
        {
            // TODO: Implement dependency injection configuration
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            // TODO: Implement authentication configuration
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}