using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using DFC.Integration.AVFeed.Function.DeleteOrphanedAVs;
using DFC.Integration.AVFeed.Repository.Sitefinity;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace DFC.Itergration.AVFeed.Function.DeleteOrphanedAVsTests
{
    public class DeleteOrphanedAVsTests
    {
        [Theory]
        [InlineData(2)]
        public async void DeleteOrphanedAvsAsyncAsyncTest(int numberOfOrphanedAVs)
        {
            //Setup
            var fakeApprenticeshipVacancyRepository = A.Fake<IApprenticeshipVacancyRepository>();
            var fakeTokenClient = A.Fake<ITokenClient>();
            var fakeApplicationLogger = A.Fake<IApplicationLogger>();
            var fakeAuditService = A.Fake<IAuditService>();

            A.CallTo(() => fakeApprenticeshipVacancyRepository.GetOrphanedApprenticeshipVacanciesAsync()).Returns(GetOrphanedTestVacancies(numberOfOrphanedAVs));
            var deleteOrphanedAVs = new DeleteOrphanedAVs(fakeApprenticeshipVacancyRepository, fakeTokenClient, fakeApplicationLogger, fakeAuditService);

            //Act
            await deleteOrphanedAVs.DeleteOrphanedAvsAsync();

            //Asserts
            A.CallTo(() => fakeApprenticeshipVacancyRepository.DeleteByIdAsync(A<Guid>._)).MustHaveHappened(Repeated.Exactly.Times(numberOfOrphanedAVs));

        }

        private IEnumerable<OrphanedVacancySummary> GetOrphanedTestVacancies(int numberOfOrphanedAVs)
        {
            for (int ii = 0; ii < numberOfOrphanedAVs; ii++)
            {
                yield return new OrphanedVacancySummary
                {
                    Id = new Guid()
                };
            }
        }
    }
}
