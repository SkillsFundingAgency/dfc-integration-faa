using Autofac;
using Castle.DynamicProxy;
using DFC.Integration.AVFeed.Core;
using NLog;

namespace DFC.Integration.AVFeed.Core
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces();
            builder.RegisterInstance(LogManager.GetLogger(nameof(DFCLogger))).As<ILogger>();

            if (builder.RunningMode() == RunMode.Console)
            {
                //Register Interceptors
                builder.RegisterType<InstrumentationInterceptor>().AsSelf().Named<IInterceptor>(InstrumentationInterceptor.Name);
                builder.RegisterType<ExceptionInterceptor>().AsSelf().Named<IInterceptor>(ExceptionInterceptor.NAME);
            }
        }
    }
}
