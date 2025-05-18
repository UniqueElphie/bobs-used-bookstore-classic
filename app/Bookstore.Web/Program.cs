
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
using Microsoft.AspNetCore.Http;
using System.Data.Entity;

namespace Bookstore
{
        public class Program
        {
            public static void Main(string[] args)
            {
                var builder = WebApplication.CreateBuilder(args);
                
                // Store configuration in static ConfigurationManager
                ConfigurationManager.Configuration = builder.Configuration;

                // Register DbContext for EntityFramework 6
                builder.Services.AddScoped<DbContext>(serviceProvider =>
                {
                    // This maintains EntityFramework 6 usage as specified in requirements
                    // In real implementation, this would use a specific DbContext type
                    return new DbContext("name=BookstoreDatabaseConnection");
                });

                // Add connection strings from web.config
                builder.Configuration["ConnectionStrings:BookstoreDatabaseConnection"] =
                    "Server=(localdb)\\MSSQLLocalDB;Initial Catalog=BookStoreClassic;MultipleActiveResultSets=true;Integrated Security=SSPI;";

                // Add app settings from web.config
                builder.Configuration["Environment"] = "Development";
                builder.Configuration["Services:Authentication"] = "local";
                builder.Configuration["Services:Database"] = "local";
                builder.Configuration["Services:FileService"] = "local";
                builder.Configuration["Services:ImageValidationService"] = "local";
                builder.Configuration["Services:LoggingService"] = "local";
                builder.Configuration["Authentication:Cognito:LocalClientId"] = "[Retrieved from AWS Systems Manager Parameter Store when Services/Authentication == 'aws']";
                builder.Configuration["Authentication:Cognito:AppRunnerClientId"] = "[Retrieved from AWS Systems Manager Parameter Store when Services/Authentication == 'aws']";
                builder.Configuration["Authentication:Cognito:MetadataAddress"] = "[Retrieved from AWS Systems Manager Parameter Store when Services/Authentication == 'aws']";
                builder.Configuration["Authentication:Cognito:CognitoDomain"] = "[Retrieved from AWS Systems Manager Parameter Store when Services/Authentication == 'aws']";
                builder.Configuration["Files:BucketName"] = "[Retrieved from AWS Systems Manager Parameter Store when Services/FileService == 'aws']";
                builder.Configuration["Files:CloudFrontDomain"] = "[Retrieved from AWS Systems Manager Parameter Store when Services/FileService == 'aws']";

                // Add services to the container (formerly ConfigureServices)
                builder.Services.AddControllersWithViews(options => {
                    // Add client-side validation settings
                    options.ModelMetadataDetailsProviders.Add(new Microsoft.AspNetCore.Mvc.ModelBinding.Metadata.ExcludeBindingMetadataProvider(typeof(System.Type)));
                });

                // Register areas (AreaRegistration.RegisterAllAreas() equivalent)
                builder.Services.Configure<RouteOptions>(options =>
                {
                    options.LowercaseUrls = true;
                    options.AppendTrailingSlash = true;
                });

                // Bundle and Minification (BundleConfig.RegisterBundles equivalent)
                // ASP.NET Core has built-in bundling and minification

                // Filter Configuration (FilterConfig.RegisterGlobalFilters equivalent)
                builder.Services.AddMvc(options =>
                {
                    // Add global filters here if needed
                });

                //Added Services
                
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

                // Configure client validation settings (from Web.config)
                app.Use(async (context, next) =>
                {
                    if (!context.Request.Path.StartsWithSegments("/api"))
                    {
                        context.Items["ClientValidationEnabled"] = true;
                    }
                    await next();
                });

                //Added Middleware
                
                app.UseRouting();

                app.UseAuthorization();

                // Global error logging (replaces Application_Error)
                app.Use(async (context, next) =>
                {
                    try
                    {
                        await next(context);
                    }
                    catch (Exception ex)
                    {
                        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An unhandled exception occurred");
                        throw;
                    }
                });
                
                // RouteConfig.RegisterRoutes equivalent
                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                // Add additional routes here if needed from the original RouteConfig
                
                app.Run();
            }
        }
        
        public class ConfigurationManager
        {
            public static IConfiguration Configuration { get; set; }
        }
    }