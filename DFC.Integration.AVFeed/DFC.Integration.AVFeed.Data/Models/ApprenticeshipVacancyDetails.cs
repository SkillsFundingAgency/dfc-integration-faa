using System;

namespace DFC.Integration.AVFeed.Data.Models
{
    public class ApprenticeshipVacancyDetails : BaseIntegrationModel
    {
        #region Feed Data
        public int VacancyReference { get; set; }
        public string Title { get; set; }
        ////public string ShortDescription { get; set; }
        ////public string Description { get; set; }
        //public string WageUnit { get; set; }
        //public string WorkingWeek { get; set; }
        public string WageText { get; set; }
        //public double HoursPerWeek { get; set; }
        //public string ExpectedDuration { get; set; }
        public string ExpectedStartDate { get; set; }
        public string PostedDate { get; set; }
        //public string ApplicationClosingDate { get; set; }
        //public string InterviewFromDate { get; set; }
        //public int NumberOfPositions { get; set; }
        //public string TrainingType { get; set; }
        //public string TrainingTitle { get; set; }
        //public string TrainingCode { get; set; }
        //public string EmployerName { get; set; }
        //public string EmployerDescription { get; set; }
        //public string EmployerWebsite { get; set; }
        //public string TrainingToBeProvided { get; set; }
        //public string QualificationsRequired { get; set; }
        //public string SkillsRequired { get; set; }
        //public string PersonalQualities { get; set; }
        //public string ImportantInformation { get; set; }
        //public string FutureProspects { get; set; }
        //public string ThingsToConsider { get; set; }
        //public bool IsNationwide { get; set; }
        //public string SupplementaryQuestion1 { get; set; }
        //public string SupplementaryQuestion2 { get; set; }
        public System.Uri VacancyUrl { get; set; }
        public AddressLocation Location { get; set; }
        //public string ContactName { get; set; }
        //public string ContactEmail { get; set; }
        //public string ContactNumber { get; set; }
        public string TrainingProviderName { get; set; }
        //public string TrainingProviderUkprn { get; set; }
        //public string TrainingProviderSite { get; set; }
        //public bool IsEmployerDisabilityConfident { get; set; }
        #endregion

        public Guid MessageId { get; set; }
        public string FrameworkCode { get; set; }
        public string VacancyLocationType { get; set; }
        public int ResultPage { get; set; }
    }

}
