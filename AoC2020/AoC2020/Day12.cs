using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2020
{
    [TestClass]
    public class Day12
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Part1()
        {
            // Iterate over lines
            var stringReader = new StringReader(DayInput);
            string line;
            var loc = (0, 0);
            var dirs = new[] {(0, 1), (1, 0), (0, -1), (-1, 0)};
            uint dir = 0;
            while ((line = stringReader.ReadLine()) != null)
            {
                var value = int.Parse(line.Substring(1));
                switch (line[0])
                {
                    case 'N':
                        loc.Item1 += value;
                        break;
                    case 'S':
                        loc.Item1 -= value;
                        break;
                    case 'W':
                        loc.Item2 -= value;
                        break;
                    case 'E':
                        loc.Item2 += value;
                        break;
                    case 'L':
                        dir = (uint) ((dir + (value / 90))) % 4;
                        break;
                    case 'R':
                        dir = (uint) ((dir - (value / 90))) % 4;
                        break;
                    case 'F':
                        loc = (loc.Item1 + dirs[dir].Item1 * value, loc.Item2 + dirs[dir].Item2 * value);
                        break;
                }
            }

            TestContext.WriteLine($"{loc}: {Math.Abs(loc.Item1) + Math.Abs(loc.Item2)}");
        }

        [TestMethod]
        public void Part2()
        {
            // Iterate over lines
            var stringReader = new StringReader(DayInput);
            string line;
            var loc = (0, 0);
            var dirs = new[] {(0, 1), (1, 0), (0, -1), (-1, 0)};
            uint dir = 0;
            var waypoint = (10, 1);
            while ((line = stringReader.ReadLine()) != null)
            {
                var value = int.Parse(line.Substring(1));
                switch (line[0])
                {
                    case 'N':
                        waypoint.Item2 += value;
                        break;
                    case 'S':
                        waypoint.Item2 -= value;
                        break;
                    case 'W':
                        waypoint.Item1 -= value;
                        break;
                    case 'E':
                        waypoint.Item1 += value;
                        break;
                    case 'L':
                        switch (value)
                        {
                            case 90:
                                waypoint = (waypoint.Item2 * -1, waypoint.Item1);
                                break;
                            case 180:
                                waypoint = (waypoint.Item1 * -1, waypoint.Item2 * -1);
                                break;
                            case 270:
                                waypoint = (waypoint.Item2, waypoint.Item1 * -1);
                                break;
                        }
                        break;
                    case 'R':
                        switch (value)
                        {
                            case 90:
                                waypoint = (waypoint.Item2, waypoint.Item1 * -1);
                                break;
                            case 180:
                                waypoint = (waypoint.Item1 * -1, waypoint.Item2 * -1);
                                break;
                            case 270:
                                waypoint = (waypoint.Item2 * -1, waypoint.Item1);
                                break;
                        }
                        break;
                    case 'F':
                        loc = (loc.Item1 + waypoint.Item1 * value, loc.Item2 + waypoint.Item2 * value);
                        break;
                }
            }

            TestContext.WriteLine($"{loc}: {Math.Abs(loc.Item1) + Math.Abs(loc.Item2)}");
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
