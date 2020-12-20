using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2020
{
    [TestClass]
    public class Day19
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Part1()
        {
            // Iterate over lines
            var stringReader = new StringReader(DayInput);
            string line;
            var rules = new Dictionary<int, string>();
            while ((line = stringReader.ReadLine()) != null)
            {
                if (line == "")
                    break;
                var strings = line.Split(':');
                rules.Add(int.Parse(strings[0]), strings[1].Trim());
            }

            var regex = new Regex($"^{GetRegex(0, rules)}$");
            // TestContext.WriteLine(regex.ToString());
            var count = 0;
            while ((line = stringReader.ReadLine()) != null)
            {
                if (regex.IsMatch(line))
                {
                    // TestContext.WriteLine(line);
                    count++;
                }
            }

            TestContext.WriteLine($"{count}");
        }

        private const int MaxRecursion = 20;

        [TestMethod]
        public void Part2()
        {
            // Iterate over lines
            var stringReader = new StringReader(DayInput);
            string line;
            var rules = new Dictionary<int, string>();
            while ((line = stringReader.ReadLine()) != null)
            {
                if (line == "")
                    break;
                if (line.StartsWith("8:"))
                    line = "8: 42 | 42 8";
                if (line.StartsWith("11:"))
                    line = "11: 42 31 | 42 11 31";
                var strings = line.Split(':');
                rules.Add(int.Parse(strings[0]), strings[1].Trim());
            }

            var regex = new Regex($"^{GetRegex(0, rules)}$");
            // TestContext.WriteLine(regex.ToString());
            var count = 0;
            while ((line = stringReader.ReadLine()) != null)
            {
                if (regex.IsMatch(line))
                {
                    // TestContext.WriteLine(line);
                    count++;
                }
            }

            TestContext.WriteLine($"{count}");
        }

        private string GetRegex(int i, Dictionary<int,string> rules, Dictionary<int, int> recursionCounter = null)
        {
            if (recursionCounter == null)
                recursionCounter = new Dictionary<int, int>();

            var rule = rules[i];
            var match = Regex.Match(rule, @"""(\w)""");
            if (match.Success)
                return match.Groups[1].Value;

            var groups = rule.Split('|');
            var stringBuilder = new StringBuilder();
            var subruleStrings = new List<string>();
            foreach (var g in groups)
            {
                var subrules = g.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries);
                var subruleStringBuilder = new StringBuilder();
                var valid = true;
                foreach (var subrule in subrules.Select(int.Parse))
                {
                    if (i == subrule)
                    {
                        if (recursionCounter.ContainsKey(subrule))
                            recursionCounter[subrule] += 1;
                        else
                            recursionCounter[subrule] = 1;

                        if (recursionCounter[subrule] > MaxRecursion)
                        {
                            valid = false;
                            break;
                        }
                    }
                    subruleStringBuilder.Append(GetRegex(subrule, rules, recursionCounter));
                }

                if (valid)
                    subruleStrings.Add(subruleStringBuilder.ToString());
            }

            stringBuilder.Append(string.Join(")|(", subruleStrings));
            if (subruleStrings.Count > 1)
                return $"(({stringBuilder}))";
            else
                return stringBuilder.ToString();
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
