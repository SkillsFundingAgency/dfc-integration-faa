﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Repository.Sitefinity.Model;
using Newtonsoft.Json;

namespace DFC.Integration.AVFeed.Repository.Sitefinity.Base
{
    public class SitefinityODataContext<T> : IOdataContext<T> where T : class, new()
    {
        private ITokenClient tokenClient;
        private readonly IApplicationLogger logger;
        private readonly IAuditService auditService;
        private readonly IHttpClientService httpClientService;
        private const string CONTENT_TYPE = "application/json";

        public SitefinityODataContext(ITokenClient tokenClient, IHttpClientService httpClient, IApplicationLogger logger, IAuditService auditService)
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

        public async Task<PagedOdataResult<T>> GetResult(Uri requestUri)
        {
            using (var client = await GetHttpClientAsync())
            {
                var result = await client.GetStringAsync(requestUri);
                logger.Info($"Requested with url - '{requestUri}'.");
                await auditService.AuditAsync($"{requestUri} | {result}");

                return JsonConvert.DeserializeObject<PagedOdataResult<T>>(result);
            }
        }

        public async Task<string> PostAsync(Uri requestUri, T data)
        {
            using (var client = await GetHttpClientAsync())
            {
                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, CONTENT_TYPE);
                var result = await client.PostAsync(requestUri, content);
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

        public async Task<string> PutAsync(Uri requestUri, string relatedEntityLink)
        {
            using (var client = await GetHttpClientAsync())
            {
                var content = new StringContent(relatedEntityLink, Encoding.UTF8, CONTENT_TYPE);
                var result = await client.PutAsync(requestUri, content);
                logger.Info($"PUT to url - '{requestUri}', returned '{result.StatusCode}' and headers '{result.Headers}'.");
                var jsonContent = await result.Content.ReadAsStringAsync();

                await auditService.AuditAsync($"PUT to url - {requestUri} | Returned - {jsonContent}");

                if (!result.IsSuccessStatusCode)
                {
                    logger.Error($"Failed to PUT url - '{requestUri}', returned '{result.StatusCode}' and headers '{result.Headers}' and content '{jsonContent}'.", new HttpRequestException(jsonContent));
                    throw new HttpRequestException(jsonContent);
                }

                return jsonContent;
            }
        }
    }
}
