﻿namespace DFC.Integration.AVFeed.Function.TestHelper
{
    using System;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;

    public static class TestUtility
    {
        public static T ReadQueue<T>(string function)
        {
            var dir = $"..\\..\\..\\DFC.Integration.AVFeed.Function.TestHelper\\Temp\\{function}\\queue\\";
            var processed = $"..\\..\\..\\DFC.Integration.AVFeed.Function.TestHelper\\Temp\\{function}\\processed\\";

            var queue = new DirectoryInfo(dir);
            var firstFile = queue.GetFiles().OrderBy(f => f.LastWriteTimeUtc).FirstOrDefault();

            if (firstFile == null)
            {
                return default(T);
            }
            Console.WriteLine($"Processing: {firstFile.Name} total left {queue.GetFiles().Count()}");
            var data = JsonConvert.DeserializeObject<T>(File.ReadAllText(firstFile.FullName));
            if (!firstFile.Name.StartsWith("sample", StringComparison.InvariantCultureIgnoreCase))
                firstFile.MoveTo($"{processed}{firstFile.Name}");

            return data;
        }

        public static void PumpResult(object result, string function)
        {
            var dir = $"..\\..\\..\\DFC.Integration.AVFeed.Function.TestHelper\\Temp\\{function}\\queue\\";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

                var sw = File.CreateText($"{dir}{Guid.NewGuid()}.json");
                using (JsonWriter jw = new JsonTextWriter(sw))
                {
                    jw.Formatting = Formatting.Indented;
                    var serializer = new JsonSerializer();
                    serializer.Serialize(jw, result);
                }
        }
    }
}