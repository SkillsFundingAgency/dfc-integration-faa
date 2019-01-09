﻿using DFC.Integration.AVFeed.Data.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Repository.Sitefinity.Base
{
    public class APIContext : IAPIContext
    {
        private ITokenClient tokenClient;
        private readonly IApplicationLogger logger;
        private readonly IAuditService auditService;
        private readonly IHttpClientService httpClientService;
        private const string CONTENT_TYPE = "application/json";

        public APIContext(ITokenClient tokenClient, IHttpClientService httpClient, IApplicationLogger logger, IAuditService auditService)
        {
            this.tokenClient = tokenClient;
            httpClientService = httpClient;
            this.logger = logger;
            this.auditService = auditService;
        }

        public async Task<HttpClient> GetHttpClientAsync()
        {
            var accessToken = await tokenClient.GetAccessTokenAsync();
            var httpClient = httpClientService.GetHttpClient();
            httpClient.SetBearerToken(accessToken);
            //ContentType = "application/json"
            httpClient.DefaultRequestHeaders.Add("X-SF-Service-Request", "true");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CONTENT_TYPE));
            return httpClient;
        }

        public async Task<string> PostAsync(Uri requestUri, Dictionary<string, string> parameters)
        {
            using (var client = await GetHttpClientAsync())
            {
                var content = new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, CONTENT_TYPE);
                var result = await client.PostAsync(requestUri, content);
                if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException(result.ReasonPhrase);
                }

                logger.Info($"Posted to url - '{requestUri}', returned '{result.StatusCode}' and headers '{result.Headers}'.");
                var jsonContent = await result.Content.ReadAsStringAsync();

                await auditService.AuditAsync($"Posted to url - {requestUri} | Returned -{jsonContent}");

                if (!result.IsSuccessStatusCode)
                {
                    logger.Error($"Failed to POST url - '{requestUri}', returned '{result.StatusCode}' and headers '{result.Headers}' and content '{jsonContent}'.", new HttpRequestException(jsonContent));
                    throw new HttpRequestException(jsonContent);
                }

                return jsonContent;
            }
        }
    }
}