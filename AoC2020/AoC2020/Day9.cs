using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2020
{
    [TestClass]
    public class Day9
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Part1()
        {
            // Iterate over lines
            var stringReader = new StringReader(DayInput);
            string line;
            var preambleSize = 25;
            var preamble = new List<long>();
            var i = 0;
            while ((line = stringReader.ReadLine()) != null)
            {
                var value = long.Parse(line);
                if (preamble.Count < preambleSize)
                {
                    preamble.Add(value);
                }
                else
                {
                    if (!CheckNumber(value, preamble))
                    {
                        TestContext.WriteLine($"{i}: {value}");
                    }

                    preamble.Add(value);
                    preamble.RemoveAt(0);
                }

                i++;
            }
        }

        [TestMethod]
        public void Part2()
        {
            var result = 507622668;
            var maxIndex = 633;

            // Iterate over lines
            var stringReader = new StringReader(DayInput);
            string line;
            var preambleSize = 25;
            var terms = new List<long>();
            var i = 0;
            while ((line = stringReader.ReadLine()) != null)
            {
                var value = long.Parse(line);
                terms.Add(value);


                while (terms.Sum() > result)
                {
                    terms.RemoveAt(0);
                }

                if (terms.Sum() == result)
                    break;

                i++;
            }

            TestContext.WriteLine($"{terms.Min() + terms.Max()}");
        }

        private bool CheckNumber(long value, List<long> preamble)
        {
            for (var i = 0; i < preamble.Count; i++)
            {
                var test = value - preamble[i];
                for (var j = i; j < preamble.Count; j++)
                {
                    if (preamble[j] == test)
                        return true;
                }
            }

            return false;
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
