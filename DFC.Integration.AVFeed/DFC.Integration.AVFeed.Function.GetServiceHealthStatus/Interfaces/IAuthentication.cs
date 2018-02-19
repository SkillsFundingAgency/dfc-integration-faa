namespace DFC.Integration.AVFeed.Function.GetServiceHealthStatus.Interfaces
{
    using Microsoft.Azure.Management.ResourceManager.Fluent;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAuthenticationHelper
    {
            AzureEnvironment AzureEnvironments { get; set; }
            Task AcquireTokens();
            Task AzLogin();
            Task<TokenSharedMemory> GetToken(string id, string resource);
            Task<TokenSharedMemory> GetTokenBySpn(string tenantId, string appId, string appKey);
            Task<TokenSharedMemory> GetTokenByUpn(string username, string password);
            bool IsCacheValid();
            void ClearTokenCache();
            IEnumerable<string> DumpTokenCache();
        }
}