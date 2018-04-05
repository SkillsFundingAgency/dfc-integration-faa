using Autofac;
using DFC.Integration.AVFeed.Core;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using DFC.Integration.AVFeed.Function.Common;

namespace DFC.Integration.AVFeed.Function.ProjectVacanciesForSoc
{
    public static class Startup
    {
        public static ILifetimeScope ConfigureContainer(RunMode mode)
        {
            var builder = ConfigureDI.ConfigureContainerWithCommonModules(mode);
            builder.RegisterModule<ProjectVacanciesForSoc.AutofacModule>();
            return builder.Build().BeginLifetimeScope();
        }

        public static ProjectedVacancySummary Run(RunMode mode, MappedVacancyDetails input)
        {
            var container = ConfigureContainer(mode);
            var getAvFunc = container.Resolve<IProjectVacanciesFunc>();
            container.Resolve<IAuditService>();
            getAvFunc.Execute(input);
            return getAvFunc.GetOutput();
        }
    }
}
