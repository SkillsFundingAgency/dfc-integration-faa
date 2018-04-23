namespace DFC.Integration.AVFeed.Data.Models
{
    public class ApprenticeshipVacancySummary
    {
        public int VacancyReference { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public System.DateTime ExpectedStartDate { get; set; }
        public System.DateTime PostedDate { get; set; }
        public System.DateTime ApplicationClosingDate { get; set; }
        public int NumberOfPositions { get; set; }
        public string TrainingType { get; set; }
        public string TrainingTitle { get; set; }
        public string TrainingCode { get; set; }
        public string EmployerName { get; set; }
        public string TrainingProviderName { get; set; }
        public bool IsNationwide { get; set; }
        public ApprenticeshipLocation Location { get; set; }
        public string ApprenticeshipLevel { get; set; }
        public System.Uri VacancyUrl { get; set; }
        public System.Uri ApiDetailUrl { get; set; }
        public bool IsEmployerDisabilityConfident { get; set; }
        public double DistanceInMiles { get; set; }
    }
}
