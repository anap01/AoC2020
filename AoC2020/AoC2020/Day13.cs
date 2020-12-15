using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2020
{
    [TestClass]
    public class Day13
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Part1()
        {
            // Iterate over lines
            var stringReader = new StringReader(DayInput);
            var line = stringReader.ReadLine();
            var timestamp = long.Parse(line);
            line = stringReader.ReadLine();
            var busIds = line.Split(',').Select(v => (int.TryParse(v, out var i), i)).Where(t => t.Item1).Select(i => i.i).ToArray();
            var mod = busIds.Select(id => (id, id - timestamp % id)).OrderBy(t => t.Item2);
            var (id, time) = mod.First();
            TestContext.WriteLine($"{id * time}");
        }

        [TestMethod]
        public void Part2()
        {
            // Iterate over lines
            var stringReader = new StringReader(DayInput);
            stringReader.ReadLine(); // Skip first line
            var line = stringReader.ReadLine();
            var busIds = line.Split(',').Select((busIdStr, index) => (int.TryParse(busIdStr, out var busId), busId, index)).Where(t => t.Item1).Select(i => (i.busId, i.index)).ToArray();
            var maxId = busIds.Max(t => t.busId);
            var maxIdIndex = busIds.First(t => t.busId == maxId).index;
            // long timestamp = -maxIdIndex;
            long timestamp = 0;
            var searching = true;
            var m = 1L;
            while (searching)
            {
                timestamp = (m*601*37) - 37;
                m++;
                searching = false;
                foreach (var (busId, index) in busIds)
                {
                    if ((timestamp + index) % busId != 0)
                    {
                        searching = true;
                        break;
                    }
                }
            }

            TestContext.WriteLine($"{timestamp}");
        }

        private string DayInput
        {
            get
            {
                using var client = new WebClient();
                client.Headers.Add(HttpRequestHeader.Cookie, Environment.GetEnvironmentVariable("sessionCookie"));
                return client.DownloadString($"https://adventofcode.com/2020/day/{this.GetType().Name.Substring(3)}/input").Trim();
            }
        }
    }
}
