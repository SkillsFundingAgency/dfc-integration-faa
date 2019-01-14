using System;
using System.Reflection;
using DFC.Integration.AVFeed.Core;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Function.TestHelper;

namespace DFC.Integration.AVFeed.Function.ClearRecycleBin.Console
{
    using Console = System.Console;
    public static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=================THE BEGINNING==============");
            try
            {
                Startup.RunAsync(RunMode.Console).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                if (ex is System.Reflection.ReflectionTypeLoadException)
                {
                    var typeLoadException = ex as ReflectionTypeLoadException;
                    var loaderExceptions = typeLoadException.LoaderExceptions;
                    for (int ii = 0; ii < loaderExceptions.Length; ii++)
                    {
                        Console.WriteLine(loaderExceptions[ii]);
                    }
                }
                else
                {
                    Console.WriteLine(ex);
                }
            }
            finally
            {
                Console.WriteLine("=================THE END==============");
                Console.ReadLine();
            }
        }
    }
}

