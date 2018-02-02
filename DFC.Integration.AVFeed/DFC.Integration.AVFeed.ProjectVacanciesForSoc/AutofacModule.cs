using Autofac;
using AutoMapper;
using DFC.Integration.AVFeed.Data.Models;

namespace DFC.Integration.AVFeed.Function.ProjectVacanciesForSoc
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces();

            builder.Register(ctx => new MapperConfiguration(cfg => cfg.CreateMap<ApprenticeshipVacancyDetails, ApprenticeshipVacancySummary>()));
            builder.Register(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper()).As<IMapper>();
        }
    }
}
