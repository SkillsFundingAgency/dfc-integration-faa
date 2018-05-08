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

            //This will still log the exceptions to insight in case of config errors with the logger
            //otherwise makes it very dificult to track.
            LogManager.ThrowExceptions = true;

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
