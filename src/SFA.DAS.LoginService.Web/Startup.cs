using System;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Application.Invitations.CreateInvitation;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Web
{
    public class Startup
    {
        private readonly IHostingEnvironment _environment;

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            _environment = environment;
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //var connectionString = "Data Source=.\\sql;Initial Catalog=SFA.DAS.LoginService;Integrated Security=True";
            var connectionString =
                "Server=tcp:esfatemp.database.windows.net,1433;Initial Catalog=SFA.DAS.LoginService;Persist Security Info=False;User ID=esfa;Password=C0ventry18;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            
            services.AddDbContext<LoginContext>(options => options.UseSqlServer(connectionString));
            
            services.AddDbContext<LoginUserContext>(options => options.UseSqlServer(connectionString));
            
            services.AddIdentity<LoginUser, IdentityRole>()
                .AddEntityFrameworkStores<LoginUserContext>()
                .AddDefaultTokenProviders();
            
            WireUpDependencies(services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            services.AddAuthentication()
                .AddJwtBearer(jwt =>
                {
                    jwt.Authority = "http://localhost:5000";
                    jwt.RequireHttpsMetadata = false;
                    jwt.Audience = "api1";
                });
            
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString);
                    options.DefaultSchema = "IdentityServer";
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString);
                    options.DefaultSchema = "IdentityServer";
                    options.EnableTokenCleanup = true;
                })
                .AddAspNetIdentity<LoginUser>();
        }

        private static void WireUpDependencies(IServiceCollection services)
        {
            services.AddTransient<ILoginConfig, LoginConfig>();
            services.AddTransient<ICodeGenerationService, CodeGenerationService>();
            services.AddTransient<IHashingService, HashingService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IUserService, UserService>();
            services.AddHttpClient<ICallbackService, CallbackService>();

            services.AddMediatR(typeof(CreateInvitationHandler).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
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
            
            app.UseIdentityServer();
            
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}