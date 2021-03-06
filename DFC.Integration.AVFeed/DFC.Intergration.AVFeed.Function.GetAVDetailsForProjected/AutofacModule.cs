﻿using Autofac;

namespace DFC.Integration.AVFeed.Function.GetAVDetailsForProjectedAV
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces();
        }
    }
}
