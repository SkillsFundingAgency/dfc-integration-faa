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
                using (var tokenClient = new TokenClient(TokenEndpoint, ClientId, ClientSecret, AuthenticationStyle.PostValues))
                {
                    logger.Info($"Token client {TokenEndpoint} called with client {ClientId}.");

                    //This is call to the token endpoint with the parameters that are set
                    var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync(Username, Password, Scopes, AdditionalParameters);
                    //var tokenResponse = await tokenClient.RequestClientCredentialsAsync("offline_access");

                    if (tokenResponse.IsError)
                    {
                        logger.Info($"Token client {TokenEndpoint} called with client {ClientId} failed with error {tokenResponse.Error}.");
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
        private static string ClientId => ConfigurationManager.AppSettings.Get("Sitefinity.ClientId");
        /// <summary>
        /// Gets the client secret.
        /// </summary>
        /// <value>
        /// The client secret.
        /// </value>
        private static string ClientSecret => ConfigurationManager.AppSettings.Get("Sitefinity.ClientSecret");
        /// <summary>
        /// Gets the token endpoint.
        /// </summary>
        /// <value>
        /// The token endpoint.
        /// </value>
        private static string TokenEndpoint => ConfigurationManager.AppSettings.Get("Sitefinity.TokenEndpoint");
        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        private static string Username => ConfigurationManager.AppSettings.Get("Sitefinity.Username");
        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        private static string Password => ConfigurationManager.AppSettings.Get("Sitefinity.Password");
        /// <summary>
        /// Gets the scopes.
        /// </summary>
        /// <value>
        /// The scopes.
        /// </value>
        private static string Scopes => ConfigurationManager.AppSettings.Get("Sitefinity.Scopes");
        /// <summary>
        /// The additional parameters
        /// </summary>
        public static readonly Dictionary<string, string> AdditionalParameters = new Dictionary<string, string>()
        {
            { "membershipProvider", "Default" }
        };
    }
}
