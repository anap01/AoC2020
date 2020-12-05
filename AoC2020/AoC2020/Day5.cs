using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2020
{
    [TestClass]
    public class Day5
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Day5Part1()
        {
            // Iterate over lines
            var stringReader = new StringReader(Day5Input);
            string line;
            var max = 0;
            var regex = new Regex(@"(\S{3}):(\S+)");
            var fields = new HashSet<string>();
            while ((line = stringReader.ReadLine()) != null)
            {
                var (row, col) = GetRowCol(line);
                max = Math.Max(max, row * 8 + col);
            }

            TestContext.WriteLine(max.ToString());
        }

        [TestMethod]
        public void Day5Part2()
        {
            // Iterate over lines
            var stringReader = new StringReader(Day5Input);
            string line;
            var list = new List<int>();
            var regex = new Regex(@"(\S{3}):(\S+)");
            var fields = new HashSet<string>();
            while ((line = stringReader.ReadLine()) != null)
            {
                var (row, col) = GetRowCol(line);
                list.Add(row * 8 + col);
            }
            list.Sort();
            var previous = list[0] - 1;
            foreach (var item in list)
            {
                if (item - previous == 1)
                {
                    previous = item;
                    continue;
                }

                TestContext.WriteLine($"previous={previous}, item={item}");
                break;
            }
        }

        private (int, int) GetRowCol(string boardingPass)
        {
            var row = 0;
            foreach (var c in boardingPass.Take(7))
            {
                row <<= 1;
                if (c == 'B')
                {
                    row += 1;
                }
            }

            var col = 0;
            foreach (var c in boardingPass.Skip(7))
            {
                col <<= 1;
                if (c == 'R')
                    col += 1;
            }

            return (row, col);
        }

        private string Day5Input
        {
            get
            {
                using var client = new WebClient();
                client.Headers.Add(HttpRequestHeader.Cookie, Environment.GetEnvironmentVariable("sessionCookie"));
                return client.DownloadString("https://adventofcode.com/2020/day/5/input");
            }
        }
    }
}
