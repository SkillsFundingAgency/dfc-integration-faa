using DFC.Integration.AVFeed.Core;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Function.TestHelper;
using System;

namespace DFC.Integration.AVFeed.Function.GetMappingsConsole
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=================THE BEGINNING==============");

            try
            {
                var result = GetMappings.Startup.RunAsync(RunMode.Console).GetAwaiter().GetResult();
                foreach (var socMapping in result.Output)
                {
                    TestUtility.PumpResult(socMapping, nameof(IGetSocMappingFunc));
                }
            }
            catch(Exception ex)
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
