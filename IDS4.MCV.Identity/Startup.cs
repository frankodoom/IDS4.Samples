using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using IDS4.AspIdentity;
using IDS4.AspIdentity.Models;
using IdentityServer4.AspNetIdentity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using IdentityServer4.Services;
using IDS4.AspIdentity.Context;
using IdentityServer4;
using Microsoft.IdentityModel.Tokens;

namespace IDS4.MCV.Identity
{
    public class Startup
    {
        //https://www.scottbrady91.com/Identity-Server/Getting-Started-with-IdentityServer-4
        //https://www.ebenmonney.com/configure-identityserver-to-use-entityframework-for-storage

        // Using Entity Framework to persist 
        //https://identityserver4.readthedocs.io/en/release/quickstarts/8_entity_framework.html

        //IDS4 Msdn
        //https://blogs.msdn.microsoft.com/webdev/2017/01/23/asp-net-core-authentication-with-identityserver4/


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionstring = @"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=IDS4;Integrated Security=True";
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            //services.AddTransient<IDatabaseInitializer, IdentityServerDbInitializer>();
            services.AddDbContext<ApplicationDbContext>(builder =>
           builder.UseSqlServer(connectionstring, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)));

            //services.AddIdentity<IdentityUser, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentity<IdentityUser, IdentityRole>()
                         .AddEntityFrameworkStores<ApplicationDbContext>()
                         .AddDefaultTokenProviders();

            services.AddMvc();

            services.TryAddScoped<IUserValidator<ApplicationUser>, UserValidator<ApplicationUser>>();
            services.TryAddScoped<IPasswordValidator<ApplicationUser>, PasswordValidator<ApplicationUser>>();
            services.TryAddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();
            services.TryAddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
            services.TryAddScoped<IRoleValidator<IdentityRole>, RoleValidator<IdentityRole>>();
            services.TryAddScoped<IdentityErrorDescriber>();
            services.TryAddScoped<ISecurityStampValidator, SecurityStampValidator<ApplicationUser>>();
            services.TryAddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>>();
            services.TryAddScoped<IUserClaimsPrincipalFactory<UserManager<IdentityUser>>, UserClaimsPrincipalFactory<UserManager<IdentityUser>, IdentityRole>>();
            services.TryAddScoped<UserManager<ApplicationUser>, AspNetUserManager<ApplicationUser>>();
            services.TryAddScoped<SignInManager<ApplicationUser>, SignInManager<ApplicationUser>>();
            services.TryAddScoped<RoleManager<IdentityRole>, AspNetRoleManager<IdentityRole>>();
            services.TryAddScoped<IRoleStore<IdentityRole>, RoleStore<IdentityRole>>();
            services.TryAddScoped<DbContext, ApplicationDbContext>();
            services.TryAddScoped<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>();
            services.AddTransient<IProfileService, ProfileService>();

            //Identity Server Builder Configuration
            services.AddIdentityServer()
            //Asp.Net Core Identity Configuration
            .AddAspNetIdentity<IdentityUser>()
             //.AddSigningCredential(new X509Certificate2(Path.Combine(".", "certs", "IdentityServer4Auth.pfx")
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

            //.AddTestUsers(Config.GetUsers())
            .AddProfileService<ProfileService>();


            //services.AddAuthentication()
            //    .AddOpenIdConnect("oidc", "OpenID Connect", options =>
            //    {
            //        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            //        options.SignOutScheme = IdentityServerConstants.SignoutScheme;
            //        options.Authority = "http://localhost:61011/";
            //        options.ClientId = "password";
            //        options.RequireHttpsMetadata = false;
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            NameClaimType = "name",
            //            RoleClaimType = "role"
            //        };
            //    });
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
           
            //Seed DB with New Configurations
            IdentityServerDbInitializer.Seed(app);

            app.UseHttpsRedirection();
            app.UseIdentityServer();
           // app.UseDefaultFiles();
            app.UseStaticFiles();
           // app.UseCookiePolicy();  
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }  
    }
}
