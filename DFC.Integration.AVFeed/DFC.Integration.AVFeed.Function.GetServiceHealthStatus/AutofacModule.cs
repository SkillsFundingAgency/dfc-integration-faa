using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Function.GetServiceHealthStatus
{
    using Autofac;
    using AutoMapper;
    using Data.Models;

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
