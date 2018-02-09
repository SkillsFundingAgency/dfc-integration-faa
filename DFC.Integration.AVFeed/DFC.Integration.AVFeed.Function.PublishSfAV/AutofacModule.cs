using Autofac;

namespace DFC.Integration.AVFeed.Function.PublishSfVacancy
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
