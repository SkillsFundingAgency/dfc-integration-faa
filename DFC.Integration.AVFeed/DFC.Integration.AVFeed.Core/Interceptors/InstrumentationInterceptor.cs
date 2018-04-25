using Castle.DynamicProxy;
using DFC.Integration.AVFeed.Data.Interfaces;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Core
{
    public class InstrumentationInterceptor : IInterceptor
    {
        private IApplicationLogger loggingService;

        public const string Name = "Instrumentation";

        public InstrumentationInterceptor(IApplicationLogger logService)
        {
            loggingService = logService;
        }

        public void Intercept(IInvocation invocation)
        {
            if (invocation != null)
            {
                var returnType = invocation.Method.ReturnType;
                if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
                {
                    loggingService.Trace(
                        $"Async Func '{invocation.Method.Name}' called with parameters '{JsonConvert.SerializeObject(invocation.Arguments)}'.");
                    invocation.Proceed();
                }
                else if (returnType == typeof(Task))
                {
                    invocation.ReturnValue = InterceptAsyncAction(invocation);
                }
                else
                {
                    InterceptSync(invocation);
                }
            }
        }

        private void InterceptSync(IInvocation invocation)
        {
            if (invocation != null)
            {
                loggingService.Trace($"Method '{invocation.Method.Name}' called with parameters '{JsonConvert.SerializeObject(invocation.Arguments)}'.");
                var watch = Stopwatch.StartNew();
                try
                {
                    invocation.Proceed();
                }
                finally
                {
                    loggingService.Trace($"Method '{invocation.Method.Name}' took '{watch.Elapsed}' to complete. And returned '{invocation.ReturnValue}'");
                }
            }
        }

        private async Task InterceptAsyncAction(IInvocation invocation)
        {
            if (invocation != null)
            {
                loggingService.Trace($"Async action '{invocation.Method.Name}' called with parameters '{JsonConvert.SerializeObject(invocation.Arguments)}'.");
                var watch = Stopwatch.StartNew();
                try
                {
                    invocation.Proceed();
                    if (invocation.ReturnValue is Task task)
                    {
                        await task;
                    }
                }
                finally
                {
                    loggingService.Trace($"Async action '{invocation.Method.Name}' took '{watch.Elapsed}' to complete.");
                }
            }
        }
    }
}
