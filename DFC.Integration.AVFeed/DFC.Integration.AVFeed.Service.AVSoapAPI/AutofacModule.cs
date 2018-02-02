using Autofac;
using AutoMapper;
using DFC.Integration.AVFeed.Data.Models;

namespace DFC.Integration.AVFeed.Service.AVSoapAPI
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var regContinuation = builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces();

            builder.Register(ctx => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<FAA.VacancyFullData, ApprenticeshipVacancyDetails>()
                .ForMember(v => v.AddressDataPostCode, a => { a.MapFrom(s => s.VacancyAddress.PostCode); })
                .ForMember(v => v.AddressDataTown, a => { a.MapFrom(s => s.VacancyAddress.Town); })
                .ForMember(v => v.County, a => { a.MapFrom(s => s.VacancyAddress.County); })
                ;
                cfg.CreateMap<ApprenticeshipVacancyDetails, ApprenticeshipVacancySummary>();
            }));

            builder.Register(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper()).As<IMapper>();
        }
    }
}
