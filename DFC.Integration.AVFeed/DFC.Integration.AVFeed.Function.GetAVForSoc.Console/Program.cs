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
            int totalDone = 0;
            int withErrors = 0;
        
            try
            {
                var input = TestUtility.ReadQueue<SocMapping>(nameof(IGetSocMappingFunc));
                while (input != null)
                {
                    totalDone++;
                    try
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
                    catch (Exception ex)
                    {
                        //if we get the standards or frameworks code not found errors continue to the next mapping
                        //by the time it gets here we will only have a 400 error response.
                        if (!ex.Message.Contains("400"))
                        {
                            throw;
                        }

                        //else process the next one
                        withErrors++;
                        Console.WriteLine($"Soc {input.SocCode} Completed execution with error 400 check standard and framework code are valid");
                        input = TestUtility.ReadQueue<SocMapping>(nameof(IGetSocMappingFunc));
                    }

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
                Console.WriteLine($"Total done {totalDone} - {withErrors} with errors");
                Console.WriteLine("=================THE END==============");
                Console.ReadLine();
            }
        }
    }
}
