using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace SFA.DAS.LoginService.Samples.MvcLocalClient.Infrastructure
{
    /// <summary>
    /// Place a flag in the context which will indicate that a custom parameter will be added to a 
    /// authorize/connect url during OpenIdConnect middleware redirects; there are probably better / more
    /// generic ways to do this; this is only a demonstration of how the application could request an
    /// addtional parameter is added when a middleware call is activated.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CreateAccountAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private static string createAccountParam = "createAccountRedirect";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            context.HttpContext.Items[createAccountParam] = bool.TrueString;
        }

        public static bool CreateAccountRedirect(HttpContext context)
        {
            return context.Items.ContainsKey(createAccountParam)
                ? context.Items[createAccountParam].ToString() == bool.TrueString
                : false;
        }
    }
}
