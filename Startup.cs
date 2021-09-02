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
using orderAPi.App_Code;

namespace orderAPi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string[] items = new string[] { };
            items = new corsorigins().connectionString();
            services.AddCors(options =>
           {
               options.AddPolicy("Bank",
                  builder => builder.WithOrigins(items[0], items[1], items[2], items[3], items[4]).WithHeaders("Accept").WithMethods("GET", "POST"));
               options.AddPolicy("Calendar",
                  builder => builder.WithOrigins(items[0], items[1], items[2], items[3], items[4]).WithHeaders("Accept").WithMethods("GET", "POST"));
               options.AddPolicy("History",
                  builder => builder.WithOrigins(items[0], items[1], items[2], items[3], items[4]).WithHeaders("Accept").WithMethods("GET"));
               options.AddPolicy("Menu",
                  builder => builder.WithOrigins(items[0], items[1], items[2], items[3], items[4]).WithHeaders("Accept").WithMethods("GET", "POST"));
               options.AddPolicy("Money",
                  builder => builder.WithOrigins(items[0], items[1], items[2], items[3], items[4]).WithHeaders("Accept").WithMethods("GET"));
               options.AddPolicy("Notifications",
                  builder => builder.WithOrigins(items[0], items[1], items[2], items[3], items[4]).WithHeaders("Accept").WithMethods("GET"));
               options.AddPolicy("Office",
                  builder => builder.WithOrigins(items[0], items[1], items[2], items[3], items[4]).WithHeaders("Accept").WithMethods("GET", "POST"));
               options.AddPolicy("Order",
                  builder => builder.WithOrigins(items[0], items[1], items[2], items[3], items[4]).WithHeaders("Accept").WithMethods("GET"));
               options.AddPolicy("Profile",
                  builder => builder.WithOrigins(items[0], items[1], items[2], items[3], items[4]).WithHeaders("Accept").WithMethods("GET"));
               options.AddPolicy("Preview",
                  builder => builder.WithOrigins(items[0], items[1], items[2], items[3], items[4]).WithHeaders("Accept").WithMethods("GET"));
               options.AddPolicy("Schedule",
                  builder => builder.WithOrigins(items[0], items[1], items[2], items[3], items[4]).WithHeaders("Accept").WithMethods("GET"));
               options.AddPolicy("Shop",
                  builder => builder.WithOrigins(items[0], items[1], items[2], items[3], items[4]).WithHeaders("Accept").WithMethods("GET", "POST"));
               options.AddPolicy("Signin",
                  builder => builder.WithOrigins(items[0], items[1], items[2], items[3], items[4]).WithHeaders("Accept").WithMethods("POST"));
               options.AddPolicy("Suggest",
                  builder => builder.WithOrigins(items[0], items[1], items[2], items[3], items[4]).WithHeaders("Accept").WithMethods("GET", "POST"));
               options.AddPolicy("Userinfo",
                  builder => builder.WithOrigins(items[0], items[1], items[2], items[3], items[4]).WithHeaders("Accept").WithMethods("GET"));
               options.AddPolicy("WeatherForecast",
                  builder => builder.WithOrigins(items[0], items[1], items[2], items[3], items[4]).WithHeaders("Accept").WithMethods("GET"));
           });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
