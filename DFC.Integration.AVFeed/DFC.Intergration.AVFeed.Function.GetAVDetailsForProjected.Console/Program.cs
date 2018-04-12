using System;
using System.Diagnostics;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using DFC.Integration.AVFeed.Function.TestHelper;


namespace DFC.Integration.AVFeed.Function.GetAVDetailsForProjectedConsole
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
                    var input = TestUtility.ReadQueue<ProjectedVacancySummary>(nameof(IProjectVacanciesFunc));
                    while (input != null)
                    {
                        var socStartTime = new Stopwatch();
                        socStartTime.Start();
                        var mappedResult = GetAVDetailsForProjected.Startup.RunAsync(input, Integration.AVFeed.Core.RunMode.Console).GetAwaiter().GetResult();
                        TestUtility.PumpResult(mappedResult, nameof(IGetAvDetailsByIdsFunc));
                       

                        Console.WriteLine($"Soc {input.SocCode} Completed execution in {socStartTime.Elapsed}");
                        socStartTime.Stop();
                        input = TestUtility.ReadQueue<ProjectedVacancySummary>(nameof(IProjectVacanciesFunc));
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
