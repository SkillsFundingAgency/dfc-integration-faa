﻿using DFC.Integration.AVFeed.Data.Models;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Data.Interfaces
{
    public  interface IPublishAVFunc
    {
        Task ExecuteAsync(ProjectedVacancySummary myQueueItem);
        PublishedVacancySummary GetOutput();
    }
}
