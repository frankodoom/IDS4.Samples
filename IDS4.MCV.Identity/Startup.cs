using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using IDS4.AspIdentity;
using IdentityServer4;

namespace IDS4.MCV.Identity
{
    public class Startup
    {
        //https://www.scottbrady91.com/Identity-Server/Getting-Started-with-IdentityServer-4        //https://www.ebenmonney.com/configure-identityserver-to-use-entityframework-for-storage/
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionstring = @"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=Test.IdentityServer4.EntityFramework;Integrated Security=True";
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            //services.AddTransient<IDatabaseInitializer, IdentityServerDbInitializer>();
            services.AddDbContext<ApplicationDbContext>(builder =>
           builder.UseSqlServer(connectionstring, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();


            services.AddMvc();

            //Identity Server Builder Configuration
            services.AddIdentityServer()

             //.AddSigningCredential("CN=sts") adding your own certificate, key material for JWT signing you can use makecert
             .AddDeveloperSigningCredential()

             //Persisiting Identity Server 4 Stores Configurations
             //add persisted grant stores
             .AddConfigurationStore(options =>
             {
                 options.ConfigureDbContext = builder =>
                  builder.UseSqlServer(connectionstring, sql =>
                                       sql.MigrationsAssembly(migrationsAssembly));
             })
             //add client and scope stores
             .AddOperationalStore(options =>
             {
                 options.ConfigureDbContext = builder =>
                   builder.UseSqlServer(connectionstring, sql =>
                                   sql.MigrationsAssembly(migrationsAssembly));

                 //this enables automatic token cleanup. this is optional. 
                 options.EnableTokenCleanup = true;
                 options.TokenCleanupInterval = 30;
             })

            //Asp.Net Core Identity Configuration
            .AddAspNetIdentity<IdentityUser>();

            //.AddProfileService<ProfileService>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseIdentityServer();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
