namespace DFC.Integration.AVFeed.Core
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    public static class HttpResponseExtensions
    {
        public static async Task EnsureSuccessStatusCodeAsync(this HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var content = await response.Content.ReadAsStringAsync();

            response.Content?.Dispose();

            throw new AvApiResponseException(response.StatusCode, content);
        }
    }

    public class AvApiResponseException : Exception
    {

        public AvApiResponseException(HttpStatusCode statusCode, string content) : base(content)
        {
            StatusCode = statusCode;
        }
        public HttpStatusCode StatusCode { get; private set; }
    }
}