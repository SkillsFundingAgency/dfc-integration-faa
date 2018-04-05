using System;

namespace DFC.Integration.AVFeed.Data.Models
{
    public class ApprenticeshipVacancyDetails : BaseIntegrationModel
    {
        #region Feed Data

        public int VacancyReference { get; set; }
        public string ContactPerson { get; set; }
        public string ContractOwner { get; set; }
        public string DeliveryOrganisation { get; set; }
        public string EmployerDescription { get; set; }
        public string EmployerWebsite { get; set; }
        public string ExpectedDuration { get; set; }
        public string FullDescription { get; set; }
        public string FutureProspects { get; set; }
        public System.DateTime InterviewFromDate { get; set; }
        public System.Nullable<bool> IsDisplayRecruitmentAgency { get; set; }
        public bool IsSmallEmployerWageIncentive { get; set; }
        public string LearningProviderDesc { get; set; }
        public System.Nullable<int> LearningProviderSectorPassRate { get; set; }
        public string OtherImportantInformation { get; set; }
        public string PersonalQualities { get; set; }
        public System.DateTime PossibleStartDate { get; set; }
        public string QualificationRequired { get; set; }
        public string SkillsRequired { get; set; }
        public string SupplementaryQuestion1 { get; set; }
        public string SupplementaryQuestion2 { get; set; }
        public string TrainingToBeProvided { get; set; }
        public string VacancyManager { get; set; }
        public string VacancyOwner { get; set; }
        public System.Nullable<decimal> Wage { get; set; }
        public string WageText { get; set; }
        public string WageType { get; set; }
        public string WorkingWeek { get; set; }
        public string ApprenticeshipFramework { get; set; }
        public System.DateTime ClosingDate { get; set; }
        public System.DateTime CreatedDateTime { get; set; }
        public string EmployerName { get; set; }
        public string LearningProviderName { get; set; }
        public int NumberOfPositions { get; set; }
        public string ShortDescription { get; set; }
        public string AddressDataPostCode { get; set; }
        public string AddressDataTown { get; set; }
        public string VacancyTitle { get; set; }
        public string VacancyType { get; set; }
        public string VacancyUrl { get; set; }
        public string County { get; set; }
        #endregion

        public Guid MessageId { get; set; }
        public string FrameworkCode { get; set; }
        public string VacancyLocationType { get; set; }
        public int ResultPage { get; set; }
    }
}
