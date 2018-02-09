using AutoMapper;
using DFC.Integration.AVFeed.Data.Models;
using DFC.Integration.AVFeed.Repository.Sitefinity.Model;

namespace DFC.Integration.AVFeed.Repository.Sitefinity.Mapper
{
    public class SitefinityDataMapper : Profile
    {
        public SitefinityDataMapper()
        {
            CreateMap<NavigateToApprenticeshipStandard, string>().ConvertUsing(r => r.UrlName.ToString());
            CreateMap<NavigateToApprenticeshipFramework, string>().ConvertUsing(r => r.UrlName.ToString());
            CreateMap<SitefinitySocMapping, SocMapping>()
                .ForMember(s => s.SocMappingId, m => m.MapFrom(d => d.Id))
                .ForMember(s => s.SocCode, m => m.MapFrom(d => d.SocCode))
                ;
        }

        public override string ProfileName => GetType().Name;
    }
}