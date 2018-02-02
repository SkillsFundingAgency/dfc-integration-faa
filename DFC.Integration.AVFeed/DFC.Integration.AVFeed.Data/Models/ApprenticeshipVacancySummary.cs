namespace DFC.Integration.AVFeed.Data.Models
{
    public class ApprenticeshipVacancySummary
    {
        public string FrameworkCode { get; set; }
        public string WageText { get; set; }
        public string VacancyUrl { get; set; }
        public string VacancyTitle { get; set; }
        public string AddressDataPostCode { get; set; }
        public string AddressDataTown { get; set; }
        public string LearningProviderName { get; set; }
        public System.DateTime PossibleStartDate { get; set; }
        public System.DateTime CreatedDateTime { get; set; }
        public int VacancyReference { get; set; }
    }
}
