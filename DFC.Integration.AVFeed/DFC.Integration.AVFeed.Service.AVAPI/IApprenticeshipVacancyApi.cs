﻿using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Service
{
    public interface IApprenticeshipVacancyApi
    {
        Task<string> GetAsync(string requestQueryString, RequestType requestTyp);
    }

    public enum RequestType
    {
        search,
        apprenticeships
    }
}