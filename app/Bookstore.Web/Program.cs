
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Routing;
using EntityFramework = System.Data.Entity;
using NLog.Extensions.Logging;

    namespace Bookstore
    {
        public class Program
        {
            public static void Main(string[] args)
            {
                var builder = WebApplication.CreateBuilder(args);

                // Configure connection string from Web.config
                builder.Services.AddScoped<EntityFramework.DbContext>(provider =>
                {
                    var connectionString = builder.Configuration.GetConnectionString("BookstoreDatabaseConnection");
                    // EntityFramework 6 connection - no EF Core migration required
                    return new EntityFramework.DbContext(connectionString);
                });

                // Add AppSettings from Web.config
                builder.Configuration.AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"ClientValidationEnabled", "true"},
                    {"Environment", "Development"},
                    {"Services:Authentication", "local"},
                    {"Services:Database", "local"},
                    {"Services:FileService", "local"},
                    {"Services:ImageValidationService", "local"},
                    {"Services:LoggingService", "local"}
                });

                // Add services to the container (formerly ConfigureServices)
                builder.Services.AddControllersWithViews(options =>
                {
                    // Enable client validation (from Web.config)
                    options.EnableEndpointRouting = true;
                });

                // Add MVC with support for areas
                builder.Services.AddControllersWithViews(options => {
                    // Equivalent of FilterConfig.RegisterGlobalFilters
                });

                // Comment out bundle support since WebOptimizer is not properly configured
                // The AddWebOptimizer method is provided by the LigerShark.WebOptimizer.Core package
                /*
                builder.Services.AddWebOptimizer(pipeline => {
                    // Here we would define bundles similar to BundleConfig.RegisterBundles
                });
                */

                // Configure logging to use NLog (from web.config runtime bindings)
                builder.Logging.ClearProviders();
                builder.Logging.AddConsole();
                builder.Logging.AddNLog();

                var app = builder.Build();
                
                // Configure the HTTP request pipeline (formerly Configure method)
                if (app.Environment.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }
                
                app.UseHttpsRedirection();
                app.UseStaticFiles();
                
                // Route configuration equivalent to RouteConfig.RegisterRoutes

                app.UseRouting();
                
                app.UseAuthorization();

                // Register routes - equivalent to RouteConfig.RegisterRoutes
                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                // Register area routes - equivalent to AreaRegistration.RegisterAllAreas()
                app.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                // Global error handler - equivalent to Application_Error
                app.UseExceptionHandler(errorApp => {
                    errorApp.Run(async context => {
                        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                        logger.LogError("An unhandled exception occurred during request processing");
                        // You can add more custom logging or error handling here
                    });
                });
                
                app.Run();
            }
        }
        

    }