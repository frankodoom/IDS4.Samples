using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace IDS4
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityServer()
                   // .AddSigningCredential("CN=sts") adding your own certificate, key material for JWT signing you can use makecert
                   .AddDeveloperSigningCredential()
                   .AddTestUsers(Config.GetUsers())
                   .AddInMemoryIdentityResources(Config.GetIdentityResources()) //OpenID resources/scopes
                   .AddInMemoryApiResources(Config.GetAllApiResources())//register api resources
                   .AddInMemoryClients(Config.GetClients()); //register clients
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
