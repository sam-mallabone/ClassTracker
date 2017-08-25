using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using APIToClassDatabase.Models;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;
using APIToClassDatabase.Schemas;

namespace APIToClassDatabase
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddDbContext<CoursesContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MyClassDatabase")));

            services.AddSwaggerGen(c =>
            {
                c.SchemaFilter<ClassTrackerSchema>();
                c.SwaggerDoc("v1",
                        new Info
                        {
                            Title = "Class Tracker - Track Due Dates",
                            Version = "v1",
                            Description = "An API connecting to a databased that tracks several due dates at a time",
                            Contact = new Contact
                            {
                                Name = "Sam Mallabone",
                                Email = "smallabo@uwo.ca"
                            },
                    });

                var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "APIToClassDatabase.xml");
                c.IncludeXmlComments(filePath);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
            app.UseStaticFiles();

            app.UseSwagger(x => 
                                x.PreSerializeFilters.Add(
                                    (swaggerDoc, httpRequest) => 
                                        swaggerDoc.Host = httpRequest.Host.Value));

            app.UseSwaggerUI(c => 
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Class Tracker - Track Due Dates");
            });
        }
    }
}
