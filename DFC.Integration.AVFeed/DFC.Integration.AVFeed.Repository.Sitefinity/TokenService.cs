using DFC.Integration.AVFeed.Data.Interfaces;
using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Function.PublishSfVacancy
{
    /// <summary>
    /// Token Service for Sitefinity Access tokens
    /// </summary>
    /// <seealso cref="ITokenClient" />
    public class TokenService : ITokenClient
    {
        private string _accessToken;
        private readonly IApplicationLogger logger;
        private readonly IAuditService service;

        public TokenService(IApplicationLogger logger, IAuditService service)
        {
            this.logger = logger;
            this.service = service;
        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ApplicationException">Couldn't get access token. Error: " + tokenResponse.Error</exception>
        public async Task<string> GetAccessTokenAsync()
        {
            if (string.IsNullOrEmpty(_accessToken))
            {
                using (var tokenClient = new TokenClient(tokenEndpoint, clientId, clientSecret, AuthenticationStyle.PostValues))
                {
                    logger.Info($"Token client {tokenEndpoint} called with client {clientId}.");

                    //This is call to the token endpoint with the parameters that are set
                    var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync(username, password, scopes, AdditionalParameters);
                    //var tokenResponse = await tokenClient.RequestClientCredentialsAsync("offline_access");

                    if (tokenResponse.IsError)
                    {
                        logger.Info($"Token client {tokenEndpoint} called with client {clientId} failed with error {tokenResponse.Error}.");
                        throw new ApplicationException("Couldn't get access token. Error: " + tokenResponse.Error);
                    }

                    _accessToken = tokenResponse.AccessToken;
                }
            }

            return _accessToken;
        }

        public void SetAccessToken(string accessToken) => _accessToken = accessToken;

        /// <summary>
        /// Gets the client identifier.
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        private string clientId => ConfigurationManager.AppSettings.Get("Sitefinity.ClientId");
        /// <summary>
        /// Gets the client secret.
        /// </summary>
        /// <value>
        /// The client secret.
        /// </value>
        public string clientSecret => ConfigurationManager.AppSettings.Get("Sitefinity.ClientSecret");
        /// <summary>
        /// Gets the token endpoint.
        /// </summary>
        /// <value>
        /// The token endpoint.
        /// </value>
        public string tokenEndpoint => ConfigurationManager.AppSettings.Get("Sitefinity.TokenEndpoint");
        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string username => ConfigurationManager.AppSettings.Get("Sitefinity.Username");
        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string password => ConfigurationManager.AppSettings.Get("Sitefinity.Password");
        /// <summary>
        /// Gets the scopes.
        /// </summary>
        /// <value>
        /// The scopes.
        /// </value>
        public string scopes => ConfigurationManager.AppSettings.Get("Sitefinity.Scopes");
        /// <summary>
        /// The additional parameters
        /// </summary>
        public static readonly Dictionary<string, string> AdditionalParameters = new Dictionary<string, string>()
        {
            { "membershipProvider", "Default" }
        };
    }
}
