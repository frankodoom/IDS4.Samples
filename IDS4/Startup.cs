using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IDS4
{
    //https://www.scottbrady91.com/Identity-Server/Getting-Started-with-IdentityServer-4
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionstring = @"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=Test.IdentityServer4.EntityFramework;Integrated Security=True";
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            //services.AddTransient<IDatabaseInitializer, IdentityServerDbInitializer>();
            services.AddIdentityServer()

             //.AddSigningCredential("CN=sts") adding your own certificate, key material for JWT signing you can use makecert
             //.AddDeveloperSigningCredential()
             //.AddTestUsers(Config.GetUsers())
             //.AddInMemoryIdentityResources(Config.GetIdentityResources()) //OpenID resources/scopes
             //.AddInMemoryApiResources(Config.GetAllApiResources())//register api resources
             //.AddInMemoryClients(Config.GetClients()); //register clients

             // The AddDeveloperSigningCredential extension creates temporary key material for signing tokens.
             // This might be useful to get started, but needs to be replaced by some persistent key material for production scenarios.
             // See http://docs.identityserver.io/en/release/topics/crypto.html#refcrypto for more information.


             //Persisiting Identity Server 4 Stores Configurations

             .AddDeveloperSigningCredential()

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
              });

               //Asp.Net Core Identity Configuration
              //.AddAspNetIdentity<ApplicationUser>()
              //.AddProfileService<ProfileService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {   
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
        }
    }
}
