using Autofac;
using AutoMapper;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Function.PublishSfVacancy;
using DFC.Integration.AVFeed.Repository.Sitefinity.Base;
using DFC.Integration.AVFeed.Repository.Sitefinity.Model;
using DFC.Integration.AVFeed.Repository.Sitefinity.Models;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Integration.AVFeed.Repository.Sitefinity
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces();

            builder.RegisterType<TokenService>().As<ITokenClient>().SingleInstance().OwnedByLifetimeScope();

            builder.RegisterType<SitefinityRepository<SitefinitySocMapping>>().As<IRepository<SitefinitySocMapping>>();
            builder.RegisterType<SitefinityRepository<SfApprenticeshipVacancy>>().As<IRepository<SfApprenticeshipVacancy>>();

            builder.RegisterType<AVSitefinityRepoEndpointConfig>().As<IRepoEndpointConfig<SfApprenticeshipVacancy>>();
            builder.RegisterType<SocRepositoryEndpointConfig>().As<IRepoEndpointConfig<SitefinitySocMapping>>();

            builder.RegisterType<SitefinityODataContext<SitefinitySocMapping>>().As<IOdataContext<SitefinitySocMapping>>();
            builder.RegisterType<SitefinityODataContext<SfApprenticeshipVacancy>>().As<IOdataContext<SfApprenticeshipVacancy>>();

            var profileTypes = typeof(AutofacModule).Assembly.GetTypes().Where(x => typeof(Profile).IsAssignableFrom(x)).ToArray();
            builder.RegisterTypes(profileTypes).As<Profile>();
            builder.Register(c => new MapperConfiguration(cfg =>
            {
                //add your profiles (either resolve from container or however else you acquire them)
                var profiles = c.Resolve<IEnumerable<Profile>>();
                foreach (var profile in profiles)
                {
                    cfg.AddProfile(profile);
                }
            }));
            builder.Register(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper()).As<IMapper>();
        }
    }
}
