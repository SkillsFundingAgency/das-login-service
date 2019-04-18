using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.LoginService.Samples.MvcLocalClient.Infrastructure;

namespace SFA.DAS.LoginService.Samples.MvcLocalClient
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
            AddLoginServiceSignInAuthorization(services);
            services.AddMvc();
        }

        public void AddLoginServiceSignInAuthorization(IServiceCollection services)
        { 
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = "Cookies";

                options.Authority = "https://localhost:5001";
                options.RequireHttpsMetadata = false;

                // see the script folder for a SQL script to insert the client details into a local database
                options.ClientId = "samples.mvclocalclient.netcore";

                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;

                options.Scope.Clear(); // remove any default scopes which are pre-populated
                options.Scope.Add("openid");
                options.Scope.Add("profile");

                options.Events = new OpenIdConnectEvents
                {
                    OnRedirectToIdentityProvider = context =>
                    {
                        // the context could contain a custom parameter which is inserted due to some
                        // application logic
                        bool createAccountRedirect = CreateAccountAuthorizeAttribute.CreateAccountRedirect(context.HttpContext);

                        if (createAccountRedirect)
                        {
                            // which will cause a custom parameter to be appended to the authorization/connect url to trigger 
                            // Create Account instead of SignIn; this will ensure that a correct returnUrl is passed to the 
                            // Login Service so that Email Confimrmation can be completed for a client which automatically 
                            // redirects to Create Account. Some applications ask the user 'Have you used this service before?'
                            // if they have then Sign-In should be displayed, if they have not Create Account should be displayed;
                            
                            // Note: It is not possible to browse to the Create Account in Login Service from a client as a correct
                            // returnUrl (OpenIdConnect format) must be present for client indentification during Email Confirmation
                            context.ProtocolMessage.SetParameter("createAccountRedirect", bool.TrueString);
                        }

                        return Task.CompletedTask;
                    }
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
