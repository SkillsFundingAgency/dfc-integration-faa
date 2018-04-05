using Autofac;
using AutoMapper;
using DFC.Integration.AVFeed.Data.Models;

namespace DFC.Integration.AVFeed.Service.AVAPI
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
