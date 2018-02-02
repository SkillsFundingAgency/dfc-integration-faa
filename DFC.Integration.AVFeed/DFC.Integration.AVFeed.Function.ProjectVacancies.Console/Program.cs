using System;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using DFC.Integration.AVFeed.Function.TestHelper;
using DFC.Integration.AVFeed.Function.ProjectVacanciesForSoc;

namespace DFC.Integration.AVFeed.Function.ProjectVacanciesConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("=================THE BEGINNING==============");
            try
            {
                var input = TestUtility.ReadQueue<MappedVacancySummary>(nameof(IGetAvForSocFunc));
                while (input != null)
                {
                    var result = Startup.Run(Core.RunMode.Console, input);
                    TestUtility.PumpResult(result, nameof(IProjectVacanciesFunc));
                    input = TestUtility.ReadQueue<MappedVacancySummary>(nameof(IGetAvForSocFunc));
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
