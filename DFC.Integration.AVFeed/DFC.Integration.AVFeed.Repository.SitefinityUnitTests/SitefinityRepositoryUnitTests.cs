using System;
using System.Net.Http;
using DFC.Integration.AVFeed.Repository.Sitefinity;
using FakeItEasy;
using Xunit;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Repository.SitefinityUnitTests
{
    public class SitefinityRepositoryUnitTests
    {
        private PagedOdataResult<SitefinitySocMapping> mappingData;

        [Fact]
        public async Task Get_all_socmappingdata_returned_by_the_repoisotryAsync()
        {
            //Arrange -- Initialize object to be constructed or interface to be injected
            mappingData = DataHelper.GetDummyOdataResultSocMapping();
            var odataRepository = A.Fake<ISocSitefinityOdataRepository>();

            //Act -- Actually should have happended with the Arranged and Act-On object
            A.CallTo(() => odataRepository.GetAllAsync()).Returns(mappingData.Value);

            //Assert - should have returned the value as expected and created in the dummy data
            Assert.Equal(mappingData.Value, await odataRepository.GetAllAsync());
        }

        [Fact]
        public void GetJsonToSocMappingMappedData()
        {
            
            mappingData = DataHelper.GetDummyOdataResultSocMapping();
            var odataclient = A.Fake<IOdataContext<SitefinitySocMapping>>();
          
            var requestUri = new Uri("http://TestUri.gov.uk");

            A.CallTo(() => odataclient.GetHttpClientAsync()).Returns(new HttpClient());
            A.CallTo(() => odataclient.GetResult(requestUri)).Returns(DataHelper.GetDummyOdataResultSocMapping());
        }
    }
}