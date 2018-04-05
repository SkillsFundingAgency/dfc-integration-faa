using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using DFC.Integration.AVFeed.Function.TestHelper;
using System;
using System.Diagnostics;

namespace DFC.Integration.AVFeed.Function.GetAVForSocConsole
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=================THE BEGINNING==============");

            var startTime = new Stopwatch();
            startTime.Start();
            try
            {
                var input = TestUtility.ReadQueue<SocMapping>(nameof(IGetSocMappingFunc));
                while (input != null)
                {
                    var socStartTime = new Stopwatch();
                    socStartTime.Start();
                    var mappedResult = GetAVForSoc.Startup.RunAsync(input, Core.RunMode.Console).GetAwaiter().GetResult();
                    TestUtility.PumpResult(mappedResult, nameof(IGetAvForSocFunc));

                    var projectedResult = ProjectVacanciesForSoc.Startup.Run(Core.RunMode.Console, mappedResult);
                    TestUtility.PumpResult(projectedResult, nameof(IProjectVacanciesFunc));

                    Console.WriteLine($"Soc {input.SocCode} Completed execution in {socStartTime.Elapsed}");
                    socStartTime.Stop();
                    input = TestUtility.ReadQueue<SocMapping>(nameof(IGetSocMappingFunc));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine($"Total time for completion {startTime.Elapsed}");
                startTime.Stop();
                Console.WriteLine("=================THE END==============");
                Console.ReadLine();
            }
        }
    }
}
