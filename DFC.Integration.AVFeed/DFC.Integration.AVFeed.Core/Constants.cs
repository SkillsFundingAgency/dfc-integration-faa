namespace DFC.Integration.AVFeed.Core
{
    using System.Configuration;

    public static  class Constants
    {
        public static string SiteFinityEndpoint=> ConfigurationManager.AppSettings.Get("Sitefinity.SocMappingEndpoint");
        public static string ApprenticeshipEndpoint => ConfigurationManager.AppSettings.Get("FAA.Endpoint");
        public static string ApprenticeshipEndpointKey=> ConfigurationManager.AppSettings.Get("FAA.PublicKey");
        public static string ExternalApplicationId => ConfigurationManager.AppSettings.Get("FAA.ExternalSystemId");
    }
}
