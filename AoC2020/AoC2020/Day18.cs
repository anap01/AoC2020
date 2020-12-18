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
    public class Day18
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Part1()
        {
            // Iterate over lines
            var stringReader = new StringReader(DayInput);
            string line;
            long sum = 0;
            while ((line = stringReader.ReadLine()) != null)
            {
                sum += Calculate(new StringReader(line + " "));
            }
            TestContext.WriteLine($"{sum}");
        }

        private static long Calculate(TextReader stringReader)
        {
            var numStr = "";
            var sum = 0L;
            var op = '+';
            int i;
            while ((i = stringReader.Read()) != -1)
            {
                var c = (char) i;
                switch (c)
                {
                    case '(':
                    case ')':
                    case ' ':
                    {
                        if (c == '(')
                            numStr = Calculate(stringReader).ToString();
                        if (string.IsNullOrEmpty(numStr) == false)
                        {
                            switch (op)
                            {
                                case '+':
                                    sum += long.Parse(numStr);
                                    break;
                                case '*':
                                    sum *= long.Parse(numStr);
                                    break;
                            }

                            numStr = "";
                        }
                        if (c == ')')
                            return sum;
                        continue;
                    }
                    case { } c2 when char.IsDigit(c2):
                        numStr += c2;
                        break;
                    default:
                        op = c;
                        break;
                }
            }

            return sum;
        }

        [TestMethod]
        public void Part2()
        {
            // Iterate over lines
            // 1 + (2 * 3) + (4 * (5 + 6))                  still becomes 51.
            // 2 * 3 + (4 * 5)                                    becomes 46.
            // 5 + (8 * 3 + 9 + 3 * 4 * 3)                      becomes 1445.
            // 5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))      becomes 669060.
            // ((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2 becomes 23340.
            var stringReader = new StringReader(DayInput);

            string line;
            var sum = 0L;
            while ((line = stringReader.ReadLine()) != null)
            {
                sum += Calculate2(new StringReader(line + " "), ' ');
            }

            TestContext.WriteLine($"{sum}");
        }

        private static long Calculate2(TextReader stringReader, char reason)
        {
            var numStr = "";
            var sum = 0L;
            var op = '+';
            int i;
            while ((i = stringReader.Read()) != -1)
            {
                var c = (char) i;
                switch (c)
                {
                    case '(':
                    case ')':
                    case ' ':
                    case '*':
                    {
                        if (c == '(' || c == '*')
                        {
                            numStr = Calculate2(stringReader, c).ToString();
                            if (c == '*')
                                op = '*';
                        }
                        if (string.IsNullOrEmpty(numStr) == false)
                        {
                            switch (op)
                            {
                                case '+':
                                    sum += long.Parse(numStr);
                                    break;
                                case '*':
                                    sum *= long.Parse(numStr);
                                    break;
                            }

                            numStr = "";
                        }
                        if (c == ')' || c == '*')
                            return sum;
                        continue;
                    }
                    case { } c2 when char.IsDigit(c2):
                        numStr += c2;
                        break;
                    default:
                        op = c;
                        break;
                }
            }

            return sum;
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
