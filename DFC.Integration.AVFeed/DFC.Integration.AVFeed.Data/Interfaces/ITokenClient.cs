using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Data.Interfaces
{
    /// <summary>
    /// Token Client
    /// </summary>
    public interface ITokenClient
    {
        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <returns></returns>
        Task<string> GetAccessTokenAsync();
        void SetAccessToken(string accessToken);
    }
}
