using Autofac;
using DFC.Integration.AVFeed.Data.Models;

namespace DFC.Integration.AVFeed.Service
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var regContinuation = builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces();
           
        }
    }
}
