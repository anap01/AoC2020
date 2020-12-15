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
