using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Function.GetServiceHealthStatus
{
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    public class TokenSharedMemory
    {
            public TokenSharedMemory()
            {
            }

            public TokenSharedMemory(string tenantId, string appId, string appKey, string resource, AuthenticationResult result)
                : this(resource, result)
            {
                AppId = appId;
                AppKey = appKey;
                TenantId = tenantId;
            }

            public TokenSharedMemory(string resource, AuthenticationResult result)
            {
                AccessToken = result.AccessToken;
                DisplayableId = result.UserInfo?.DisplayableId;
                ExpiresOn = result.ExpiresOn;
                RefreshToken = result.RefreshToken;
                Resource = resource;
                TenantId = result.TenantId;
            }

            public string AppId { get; set; }
            public string AppKey { get; set; }
            public string AccessToken { get; set; }
            public string DisplayableId { get; set; }
            public DateTimeOffset ExpiresOn { get; set; }
            public string RefreshToken { get; set; }
            public string Resource { get; set; }
            public string TenantId { get; set; }
            public string ClientId { get; set; }

            public string CreateAuthorizationHeader()
            {
                return String.Format("Bearer {0}", AccessToken);
            }
        }

}
