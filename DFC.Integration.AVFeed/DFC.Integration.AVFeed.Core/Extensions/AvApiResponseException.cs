namespace DFC.Integration.AVFeed.Core
{
    using System;
    using System.Net;

    public class AvApiResponseException : Exception
    {

        public AvApiResponseException(HttpStatusCode statusCode, string content) : base(content)
        {
            StatusCode = statusCode;
        }
        public HttpStatusCode StatusCode { get; private set; }
    }
}