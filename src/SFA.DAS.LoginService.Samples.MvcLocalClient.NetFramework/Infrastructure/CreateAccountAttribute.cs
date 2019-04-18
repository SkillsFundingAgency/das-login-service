using System;
using System.Web;
using System.Web.Mvc;


namespace SFA.DAS.LoginService.Samples.MvcLocalClient.NetFramework.Infrastructure
{
    /// <summary>
    /// Place a flag in the context which will indicate that a custom parameter will be added to a 
    /// authorize/connect url during OpenIdConnect middleware redirects; there are probably better / more
    /// generic ways to do this; this is only a demonstration of how the application could request an
    /// addtional parameter is added when a middleware call is activated.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.AuthorizeAttribute" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CreateAccountAuthorizeAttribute : AuthorizeAttribute
    {
        private static string createAccountParam = "createAccountRedirect";

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Items[createAccountParam] = bool.TrueString;
            base.OnAuthorization(filterContext);
        }

        public static bool CreateAccountRedirect(HttpContextBase context)
        {
            return context.Items.Contains(createAccountParam)
                ? context.Items[createAccountParam].ToString() == bool.TrueString
                : false;
        }
    }
}