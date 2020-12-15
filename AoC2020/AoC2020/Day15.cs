using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2020
{
    [TestClass]
    public class Day15
    {
        private int[] m_numbers;
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Part1()
        {
            m_numbers = new int[2020];
            var start = new[] {9, 12, 1, 4, 17, 0, 18};
            var i = start.Length;
            start.CopyTo(m_numbers, 0);
            while (i < 2020)
            {
                var distance = GetDistance(i - 1);
                m_numbers[i] = distance;
                i++;
            }

            TestContext.WriteLine($"{m_numbers[2019]}");
        }

        [TestMethod]
        public void Part2()
        {
            const int limit = 30000000;
            var start = new[] {9,12,1,4,17,0,18}; //{9, 12, 1, 4, 17, 0, 18};
            var numbers = start.Take(start.Length - 1).Select((n, i) => (n, i)).ToDictionary(k => k.n, v => v.i);
            var i = start.Length - 1;
            var previous = start.Last();
            while (i < limit - 1)
            {
                previous = GetDistance2(previous,i, numbers);
                i++;
            }

            TestContext.WriteLine($"{previous}");
        }

        private static int GetDistance2(int value, int index, IDictionary<int, int> numbers)
        {
            if (numbers.TryGetValue(value, out var max))
            {
                numbers[value] = index;
                return index - max;
            }

            numbers[value] = index;
            return 0;
        }

        private int GetDistance(int index)
        {
            var value = m_numbers[index];
            for (var j = index - 1; j >= 0; j--)
            {
                if (m_numbers[j] == value)
                    return index - j;
            }

            return 0;
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
