using DFC.Integration.AVFeed.Service.AVSoapAPI.FAA;
using System;
using System.Configuration;
using System.ServiceModel;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Service.AVSoapAPI
{
    public class ClientProxy : IVacancyDetailsSoapApi
    {
        private Guid _externalSystemId = Guid.Parse(ConfigurationManager.AppSettings.Get("FAA.ExternalSystemId"));
        private string _publicKey = ConfigurationManager.AppSettings.Get("FAA.PublicKey");
        private string _endpoint = ConfigurationManager.AppSettings.Get("FAA.Endpoint");
        private long _maxReceivedMessageSize = long.Parse(ConfigurationManager.AppSettings.Get("FAA.MaxReceivedMessageSize"));
        private int _maxBufferSize = int.Parse(ConfigurationManager.AppSettings.Get("FAA.MaxBufferSize"));

        public async Task<VacancyDetailsResponse> GetAsync(VacancyDetailsRequest vacancyDetailsRequest)
        {
            using (var clientProxy = this.GetClient())
            {
                vacancyDetailsRequest.ExternalSystemId = _externalSystemId;
                vacancyDetailsRequest.PublicKey = _publicKey;
                vacancyDetailsRequest.MessageId = Guid.NewGuid();

                return await clientProxy.GetAsync(vacancyDetailsRequest);
            }
        }

        private VacancyDetailsClient GetClient()
        {
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport)
            {
                MaxReceivedMessageSize = long.Parse(ConfigurationManager.AppSettings.Get("FAA.MaxReceivedMessageSize")),
                MaxBufferSize = int.Parse(ConfigurationManager.AppSettings.Get("FAA.MaxBufferSize"))
            };
            return new VacancyDetailsClient(binding, new EndpointAddress(_endpoint));
        }

    }
}
