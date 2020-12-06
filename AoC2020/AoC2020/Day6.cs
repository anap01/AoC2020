using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2020
{
    [TestClass]
    public class Day6
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Day6Part1()
        {
            // Iterate over lines
            var stringReader = new StringReader(Day6Input);
            string line;
            var sum = 0;

            var hash = new HashSet<char>();
            while ((line = stringReader.ReadLine()) != null)
            {
                if (line == "")
                {
                    sum += hash.Count;
                    hash.Clear();
                }

                foreach (var c in line)
                {
                    hash.Add(c);
                }
            }

            sum += hash.Count;
            TestContext.WriteLine(sum.ToString());
        }

        [TestMethod]
        public void Day6Part2()
        {
            // Iterate over lines
            var stringReader = new StringReader(Day6Input);
            string line;
            var sum = 0;
            var size = 0;
            var dict = new Dictionary<char, int>();
            while ((line = stringReader.ReadLine()) != null)
            {
                if (line == "")
                {
                    sum += dict.Count(kv => kv.Value == size);
                    dict.Clear();
                    size = 0;
                    continue;
                }

                foreach (var c in line)
                {
                    if (dict.TryGetValue(c, out var current))
                    {
                        dict[c]++;
                    }
                    else
                    {
                        dict.Add(c, 1);
                    }
                }
                size++;
            }

            sum += dict.Count(kv => kv.Value == size);
            TestContext.WriteLine(sum.ToString());
        }

        private string Day6Input
        {
            get
            {
                using var client = new WebClient();
                client.Headers.Add(HttpRequestHeader.Cookie, Environment.GetEnvironmentVariable("sessionCookie"));
                return client.DownloadString("https://adventofcode.com/2020/day/6/input");
            }
        }
    }
}
