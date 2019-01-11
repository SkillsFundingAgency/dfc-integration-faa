using System;
using DFC.Integration.AVFeed.Core;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Function.TestHelper;

namespace DFC.Integration.AVFeed.Function.GetServiceHealthStatus.Console
{
    using Console = System.Console;

    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=================THE BEGINNING==============");

            try
            {
                var result = Startup.RunAsync(RunMode.Console).GetAwaiter().GetResult();
                {
                    TestUtility.PumpResult(result, nameof(IClearRecycleBin));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("=================THE END==============");
                Console.ReadLine();
            }
        }
    }
}
