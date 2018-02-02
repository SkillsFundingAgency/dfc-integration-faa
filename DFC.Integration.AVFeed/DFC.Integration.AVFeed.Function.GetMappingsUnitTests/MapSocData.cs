using AutoMapper;
using DFC.Integration.AVFeed.Data.Models;
using DFC.Integration.AVFeed.Repository.Sitefinity.Model;

namespace DFC.Integration.AVFeed.Function.GetMappingsUnitTests
{
    public static class MapSocData
    {
        public static IMapper MapperData;
        public  static void Configure()
        {

             Mapper.Initialize(cfg =>
            {

                cfg.CreateMap<NavigateToApprenticeshipStandard, string>().ConvertUsing(r => r.UrlName.ToString());
                cfg.CreateMap<NavigateToApprenticeshipFramework, string>().ConvertUsing(r => r.UrlName.ToString());
                cfg.CreateMap<SitefinitySocMapping, SocMapping>()
                    .ForMember(s => s.SocCode, m => m.MapFrom(d => d.SocCode))
                    .ForMember(s=>s.CorrelationId,m=>m.MapFrom(d=>d.Id))
                    .ForMember(s=>s.SocMappingId,m=>m.MapFrom(d=>d.Id))
                    .ReverseMap();
            });
            
            //config.AssertConfigurationIsValid();
        }
    }
}
