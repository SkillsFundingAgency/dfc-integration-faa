using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using DFC.Integration.AVFeed.Service.AVSoapAPI.FAA;
using FakeItEasy;
using System;

namespace DFC.Integration.AVFeed.Service.AVSoapAPI.Tests
{
    public class AVSoapWebServiceTests
    {

        //Cannot get this to work as we cannot fake the SOAP client
        public void GetApprenticeshipVacancyDetailsTest()
        {
            var mapping = new SocMapping
            {
                SocCode = "1234",
                SocMappingId = Guid.NewGuid(),
                Standards = new string[] { "S1", "S2" },
                Frameworks = new string[] { "F1", "F2" }
            };

            var dummyVacancyDetailsResponse = A.Dummy<VacancyDetailsResponse>();

            var fakeSoapApi = A.Fake<IVacancyDetailsSoapApi>();
            var fakeLogger = A.Fake<IApplicationLogger>();
           
            var avService = new AVSoapWebService(fakeSoapApi, null, fakeLogger);
            var result = avService.GetApprenticeshipVacancyDetails(mapping);

            A.CallTo(() => fakeSoapApi.GetAsync(A<VacancyDetailsRequest>._)).MustHaveHappened(Repeated.Exactly.Times(7));
        }



        private VacancyDetailsResponse GetFakeVacancies ()
        {
            var retResponse = new VacancyDetailsResponse();
            retResponse.SearchResults.TotalPages = 1;

            var retVacancies = new VacancyFullData[]
            {
                new VacancyFullData()
            };

            retResponse.SearchResults.SearchResults = retVacancies;

            return retResponse;
        }

    }
}