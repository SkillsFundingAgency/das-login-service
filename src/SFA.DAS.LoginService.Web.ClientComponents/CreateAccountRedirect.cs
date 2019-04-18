using System;
using System.Web;

namespace SFA.DAS.LoginService.Web.ClientComponents
{
    public class CreateAccountRedirect
    {
        public const string CreateAccountRedirectParam = "createAccountRedirect";

        /// <summary>
        /// Gets the value of the createAccountRedirect param from the query string if it exists.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>True if the createAccountRedirect param exists and is set true; false otherwise.</returns>
        public static bool GetCreateAccountRedirect(string returnUrl)
        {
            var queryCollection = HttpUtility.ParseQueryString(new Uri(new Uri("http://uri"), returnUrl).Query);

            bool createAccountRedirect;
            return bool.TryParse(queryCollection[CreateAccountRedirectParam], out createAccountRedirect) == true
                ? createAccountRedirect
                : false;
        }

        /// <summary>
        /// Sets the createAccountRedirect param of the query string if it exists to the given value.
        /// </summary>
        /// <param name="returnUrl">The return URL which may contain a createAccountRedirect param.</param>
        /// <param name="redirect">When true createAccountRedirect param will be set true; otherwise createAccountRedirect will set false.</param>
        /// <returns>The query string with the createAccountRedirect param if it exists set to the given value.</returns>
        public static string SetCreateAccountRedirect(string returnUrl, bool redirect)
        {
            var uri = new Uri(new Uri("http://uri"), returnUrl);

            var query = HttpUtility.ParseQueryString(uri.Query);
            if(query[CreateAccountRedirectParam] != null)
            {
                query[CreateAccountRedirectParam] = redirect.ToString();
            }

            // query is a HttpValueCollection which will format to a valid query string
            return $"{uri.AbsolutePath}?{query.ToString()}";
        }
    }
}
