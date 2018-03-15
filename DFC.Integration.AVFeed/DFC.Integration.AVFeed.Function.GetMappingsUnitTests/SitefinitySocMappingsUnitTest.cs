using System.Threading.Tasks;
using DFC.Integration.AVFeed.Data.Interfaces;
using FakeItEasy;
using Xunit;

namespace DFC.Integration.AVFeed.Function.GetMappingsUnitTests
{
    public class SitefinitySocMappingsUnitTest:BaseInitialization
    {
        [Fact]
        public void Get_output_when_execute_is_called_on_socmappingRepository()
        {
            var iGetSocMappingFunction = A.Fake<IGetSocMappingFunc>();

            iGetSocMappingFunction.Execute();
            iGetSocMappingFunction.GetOutput();

            A.CallTo(() => iGetSocMappingFunction.Execute()).Returns(Task.FromResult(0));
            A.CallTo(() => iGetSocMappingFunction.GetOutput()).Returns(DataHelper.GetFinalSocMappingData());

        }

        [Fact]
        public void Get_mapped_Data_from_the_sitefinity_JsonObject_to_MapperMappedSocMapping()
        {
            var iSocMappingRepository = A.Fake<ISocMappingRepository>();

             A.CallTo(() => iSocMappingRepository.GetSocMappingAsync()).Returns(Task.FromResult(DataHelper.GetFinalSocMappingData()));
        }
    }
}