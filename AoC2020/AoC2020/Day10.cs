using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2020
{
    [TestClass]
    public class Day10
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Part1()
        {
            // Iterate over lines
            var stringReader = new StringReader(DayInput);
            string line;
            var adapters = new List<int>();
            while ((line = stringReader.ReadLine()) != null)
            {
                var value = int.Parse(line);
                adapters.Add(value);
            }
            adapters.Sort();
            var steps = new int[3];
            var currentJoltage = 0;
            foreach (var adapter in adapters)
            {
                var delta = adapter - currentJoltage;
                steps[delta - 1]++;
                currentJoltage = adapter;
            }

            steps[2]++;
            TestContext.WriteLine($"1: {steps[0]}\n3: {steps[2]}\nProduct: {steps[0] * steps[2]}");
        }

        [TestMethod]
        public void Part2()
        {
            // Iterate over lines
            var stringReader = new StringReader(DayInput);
            string line;
            var adapters = new List<int>();
            while ((line = stringReader.ReadLine()) != null)
            {
                var value = int.Parse(line);
                adapters.Add(value);
            }
            adapters.Sort();
            var currentJoltage = 0;
            var numConsecutive = 1;
            var combinations = 1L;
            foreach (var adapter in adapters)
            {
                var delta = adapter - currentJoltage;
                if (delta == 1)
                {
                    numConsecutive++;
                }
                else
                {
                    // 1, 2, 3 => *2
                    // 1, 2, 3, 4 => *4
                    // 1, 2, 3, 4, 5 => *7
                    // 1, 2, 3, 4, 5, 6 => *10
                    // 1, 2, 3, 4, 5, 6, 7 => *13
                    // 1, 2, 3, 4, 5, 6, 7, 8 => *16
                    if (numConsecutive > 2)
                        combinations *= 1 + Math.Max(1, (numConsecutive - 3) * 3);
                    numConsecutive = 1;
                }

                currentJoltage = adapter;
            }

            // And the last one
            if (numConsecutive > 2)
                combinations *= 1 + Math.Max(1, (numConsecutive - 3) * 3);

            TestContext.WriteLine($"{combinations}");
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
