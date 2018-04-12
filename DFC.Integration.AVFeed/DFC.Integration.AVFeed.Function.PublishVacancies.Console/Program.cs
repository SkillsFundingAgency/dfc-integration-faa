using System;
using DFC.Integration.AVFeed.Function.PublishSfVacancy;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using DFC.Integration.AVFeed.Function.TestHelper;

namespace DFC.Integration.AVFeed.Function.PublishVacanciesConsole
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=================THE BEGINNING==============");
            try
            {

                var input = TestUtility.ReadQueue<ProjectedVacancyDetails>(nameof(IGetAvDetailsByIdsFunc));
                while (input != null)
                {
                    var result = Startup.RunAsync(input, Core.RunMode.Console).GetAwaiter().GetResult();
                    TestUtility.PumpResult(result, nameof(IPublishAVFunc));
                    input = TestUtility.ReadQueue<ProjectedVacancyDetails>(nameof(IGetAvDetailsByIdsFunc));
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
