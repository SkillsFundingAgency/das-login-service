using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using IdentityServer4.Configuration;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Application.Invitations.CreateInvitation;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            
            WireUpDependencies(services);
            
            services.AddDbContext<LoginContext>(options => options.UseSqlServer(_loginConfig.SqlConnectionString));
            services.AddDbContext<LoginUserContext>(options => options.UseSqlServer(_loginConfig.SqlConnectionString));

            AddIdentityServer(services);

            services.AddAuthentication()
                .AddJwtBearer(jwt =>
                {
                    jwt.Authority = "http://localhost:5000";
                    jwt.RequireHttpsMetadata = false;
                    jwt.Audience = "api1";
                });
        }

        private void AddIdentityServer(IServiceCollection services)
        {
            services.AddIdentity<LoginUser, IdentityRole>(
                    options =>
                    {
                        options.Password.RequiredLength = 8;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireUppercase = false;
                    })
                .AddEntityFrameworkStores<LoginUserContext>()
                .AddDefaultTokenProviders();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var isBuilder = services.AddIdentityServer().AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseSqlServer(_loginConfig.SqlConnectionString);
                    options.DefaultSchema = "IdentityServer";
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseSqlServer(_loginConfig.SqlConnectionString);
                    options.DefaultSchema = "IdentityServer";
                    options.EnableTokenCleanup = true;
                })
                .AddAspNetIdentity<LoginUser>();

            if (_environment.IsDevelopment())
            {
                isBuilder.AddDeveloperSigningCredential();
            }
            else
            {
                isBuilder.AddCertificateFromStore(_loginConfig.CertificateThumbprint, _logger);
            }
        }


        private void WireUpDependencies(IServiceCollection services)
        {
            services.AddTransient<IConfigurationService, ConfigurationService>();
            
            _loginConfig = new ConfigurationService()
                .GetLoginConfig(
                    Configuration["EnvironmentName"], 
                    Configuration["ConfigurationStorageConnectionString"],
                    "1.0",
                    "SFA.DAS.LoginService", _environment).Result;
            
            services.AddTransient(sp => _loginConfig);

            if (_environment.IsDevelopment())
            {
                services.AddTransient<IEmailService, EmailService.DevEmailService>();
            }
            else
            {
                services.AddHttpClient<IEmailService, EmailService.EmailService>();    
            }
            
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<CustomSignInManager>();
            services.AddHttpClient<ICallbackService, CallbackService>()
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(GetRetryPolicy());

            services.AddMediatR(typeof(CreateInvitationHandler).Assembly);
        }

        private IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            var jitterer = new Random();
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
                .WaitAndRetryAsync(5, 
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) 
                                    + TimeSpan.FromMilliseconds(jitterer.Next(0, 100)));
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
                app.UseExceptionHandler("/Error");
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