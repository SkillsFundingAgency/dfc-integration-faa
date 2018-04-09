using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Function.PublishSfVacancy
{
    public class PublishSfVacancyFunction: IPublishAVFunc
    {

        private readonly IApprenticeshipVacancyRepository apprenticeshipVacancyRepository;
        private readonly ITokenClient sitefinityTokenClient;
        private readonly IApplicationLogger logger;
        private PublishedVacancySummary output;

        public PublishSfVacancyFunction(IApprenticeshipVacancyRepository apprenticeshipVacancyRepository, ITokenClient sitefinityTokenClient, IApplicationLogger logger)
        {
            this.apprenticeshipVacancyRepository = apprenticeshipVacancyRepository;
            this.sitefinityTokenClient = sitefinityTokenClient;
            this.logger = logger;
        }

        public async Task ExecuteAsync(ProjectedVacancySummary myQueueItem)
        {
            List<PublishedAV> publishedVacancies = new List<PublishedAV>();

            sitefinityTokenClient.SetAccessToken(myQueueItem.AccessToken);
            await apprenticeshipVacancyRepository.DeleteExistingAsync(myQueueItem.SocMappingId);
            logger.Info($"Deleted all vacancies for soc code {myQueueItem.SocCode}, ready for publishing {myQueueItem.Vacancies.Count()} vacancies.");

            foreach (var vacancy in myQueueItem.Vacancies)
            {
                var urlName = await apprenticeshipVacancyRepository.PublishAsync(vacancy, myQueueItem.SocMappingId);
                publishedVacancies.Add(new PublishedAV
                {
                    UrlName = urlName,
                    VacancyReference = vacancy.VacancyReference,
                    Title = vacancy.Title,
                    VacancyUrl = vacancy.VacancyUrl
                });
            }

            output = new PublishedVacancySummary
            {
                SocCode = myQueueItem.SocCode,
                SocMappingId = myQueueItem.SocMappingId,
                Vacancies = publishedVacancies,
            };
        }

        public PublishedVacancySummary GetOutput()
        {
            return output;
        }
    }
}
