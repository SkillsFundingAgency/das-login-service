using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SFA.DAS.LoginService.Application.Services.Configuration;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Web.Infrastructure;

namespace SFA.DAS.LoginService.Web
{
    public class Startup
    {
        private readonly IHostingEnvironment _environment;
        private readonly ILogger<Startup> _logger;
        private ILoginConfig _loginConfig;

        public Startup(IConfiguration configuration, IHostingEnvironment environment, ILogger<Startup> logger)
        {
            _environment = environment;
            _logger = logger;
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            _loginConfig = services.WireUpDependencies(_loginConfig, Configuration, _environment);

            services.AddDbContext<LoginContext>(options => options.UseSqlServer(_loginConfig.SqlConnectionString));
            services.AddDbContext<LoginUserContext>(options => options.UseSqlServer(_loginConfig.SqlConnectionString));

            services.AddIdentityServer(_loginConfig, _environment, _logger);

            services.AddAntiforgery(options => options.Cookie = new CookieBuilder() {Name = ".Login.AntiForgery", HttpOnly = true});

            services.AddAuthentication()
                .AddJwtBearer(jwt =>
                {
                    jwt.Authority = "http://localhost:5000";
                    jwt.RequireHttpsMetadata = false;
                    jwt.Audience = "api1";
                });
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");       
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