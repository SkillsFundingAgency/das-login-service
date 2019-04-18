using System;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using SFA.DAS.LoginService.Samples.MvcLocalClient.NetFramework.Infrastructure;

[assembly: OwinStartup(typeof(SFA.DAS.LoginService.Samples.MvcLocalClient.NetFramework.Startup))]

namespace SFA.DAS.LoginService.Samples.MvcLocalClient.NetFramework
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies",
                ExpireTimeSpan = new TimeSpan(0, 10, 0),
                SlidingExpiration = true
            });
           
            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                AuthenticationType = "oidc",
                SignInAsAuthenticationType = "Cookies",
                Authority = "https://localhost:5001",
                // This must be present and match the [ClientRedirectUrls] entry for the [Client]
                RedirectUri = "https://localhost:44353/signin-oidc",
                // this is optional if present it must match the [ClientPostRedirectUrls] entry for the [Client]; in .Net Core
                // these usually end with '/signout-callback-oidc' and a state parameter is appended automatically which
                // redirects back to the logout url via the identity provider - however this doesn't work using the Owin
                // library - leaving the Uri as the base address of the client does generate a correct Uri
                PostLogoutRedirectUri = "https://localhost:44353/",
                ClientId = "samples.mvclocalclient.netframework",
                ResponseType = "id_token",
                Scope = "openid profile",
                UseTokenLifetime = true,
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = n =>
                    {
                        var id = n.AuthenticationTicket.Identity;

                        // ensure that the id_token is stored in a claim for use in logout; in .Net Core
                        // RemoteAuthenticationOptions.SaveTokens cause the id_token to stored
                        id.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));
                        n.AuthenticationTicket = new AuthenticationTicket(
                            id,
                            n.AuthenticationTicket.Properties);

                        return Task.FromResult(0);
                    },

                    RedirectToIdentityProvider = n =>
                    {
                        if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.Logout)
                        {
                            // retrieve the stored id_token and set a hint so a correct logout token will be generated; in .Net Core
                            // RemoteAuthenticationOptions.SaveTokens cause the id_token to retrieved
                            var idTokenHint = n.OwinContext.Authentication.User.FindFirst("id_token").Value;
                            n.ProtocolMessage.IdTokenHint = idTokenHint;
                        }
                        else if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.Authentication)
                        {
                            // the context could contain a custom parameter which is inserted due to some
                            // application logic
                            HttpContextBase httpContext = n.OwinContext.Get<HttpContextBase>(typeof(HttpContextBase).FullName);
                            bool createAccountRedirect = CreateAccountAuthorizeAttribute.CreateAccountRedirect(httpContext);

                            if (createAccountRedirect)
                            {
                                // which will cause a custom parameter to be appended to the authorization/connect url to trigger 
                                // Create Account instead of SignIn; this will ensure that a correct returnUrl is passed to the 
                                // Login Service so that Email Confimrmation can be completed for a client which automatically 
                                // redirects to Create Account. Some applications ask the user 'Have you used this service before?'
                                // if they have then Sign-In should be displayed, if they have not Create Account should be displayed;

                                // Note: It is not possible to browse to the Create Account in Login Service from a client as a correct
                                // returnUrl (OpenIdConnect format) must be present for client indentification during Email Confirmation
                                n.ProtocolMessage.SetParameter("createAccountRedirect", bool.TrueString);
                            }
                        }

                        return Task.FromResult(0);
                    }
                }
            });
        }
    }
}