using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using DFC.Integration.AVFeed.Core.Interceptors;
using Autofac.Extras.DynamicProxy;
using DFC.Integration.AVFeed.Core.Extensions;

namespace DFC.Integration.AVFeed.Repository.Audit
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            var regContinuation = builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces();

            if (builder.RunningMode() == Core.RunMode.Console)
            {
                regContinuation
                    .EnableInterfaceInterceptors()
                    .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME);
            }
        }
    }
}
