using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IDS4.WebApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IDS4.TestApi
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
            services.AddIdentity<IdentityUser, IdentityRole>()
                         .AddEntityFrameworkStores<ApplicaionDbContext>()
                         .AddDefaultTokenProviders();

            //Configure Authentication
            services.AddAuthentication("Bearer")
               .AddIdentityServerAuthentication(option =>
               {
                   option.Authority = "http://localhost:61011";
                   option.RequireHttpsMetadata = false;
                   option.ApiName = "Cloud911Api";
                   option.ApiSecret = "secret";
               });

            //Configure Db
            var connectionstring = @"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=IDS4;Integrated Security=True";
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContext<ApplicaionDbContext>(builder =>
            builder.UseSqlServer(connectionstring, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMvc();
            
        }
    }
}
