using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.LoginService.Web.Infrastructure
{
    public static class IdentityServerExtensions
    {
        public static void AddCertificateFromStore(this 
                IIdentityServerBuilder builder, 
            string thumbprint, 
            ILogger logger)
        {
            var certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            certStore.Open(OpenFlags.ReadOnly);
            var certCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint, thumbprint,false);

            if (certCollection.Count > 0)
                builder.AddSigningCredential(certCollection[0]);
            else
                logger.LogError("A matching key couldn't be found in the store");
            
            certStore.Close();
        }
    }
}