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
    public class Day17
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Part1()
        {
            // Iterate over lines
            var stringReader = new StringReader(DayInput);
            string line;
            var space = new Dictionary<(int, int, int), char>();
            var y = 0;
            while ((line = stringReader.ReadLine()) != null)
            {
                foreach (var (c, x) in line.Select((c, i) => (c, i)))
                {
                    space.Add((x, y, 0), c);
                }

                y++;
            }

            Simulate(6, space);

            TestContext.WriteLine($"{space.Count(c => c.Value == '#')}");
        }

        private void Simulate(int cycles, Dictionary<(int, int, int),char> space)
        {
            for (var i = 0; i < cycles; i++)
            {
                Simulate(space);
            }
        }

        private void Simulate(Dictionary<(int, int, int),char> space)
        {
            AddEmptyNeighbours(space);
            var actions = new List<Action>();
            foreach (var cube in space)
            {
                var neighbours = CountNeighbours(cube, space);
                switch (cube.Value)
                {
                    case '#' when (neighbours < 2 || neighbours > 3):
                        actions.Add(() => space[cube.Key] = '.');
                        break;
                    case '.' when neighbours == 3:
                        actions.Add(() => space[cube.Key] = '#');
                        break;
                }
            }
            actions.ForEach(a => a.Invoke());
        }

        private int CountNeighbours(KeyValuePair<(int, int, int),char> cube, Dictionary<(int, int, int),char> space)
        {
            var neighbours = 0;
            for (var dx = -1; dx <= 1; dx++)
            {
                for (var dy = -1; dy <= 1; dy++)
                {
                    for (var dz = -1; dz <= 1; dz++)
                    {
                        if ((dx, dy, dz) == (0, 0, 0))
                            continue;

                        var (x, y, z) = cube.Key;
                        var key = (x + dx, y + dy, z + dz);
                        if (!space.TryGetValue(key, out var c))
                            continue;

                        if (c == '#')
                            neighbours++;
                    }
                }
            }

            return neighbours;
        }

        private void AddEmptyNeighbours(Dictionary<(int, int, int),char> space)
        {
            foreach (var cube in space.Where(kvp => kvp.Value == '#').ToList())
            {
                for (var dx = -1; dx <= 1; dx++)
                {
                    for (var dy = -1; dy <= 1; dy++)
                    {
                        for (var dz = -1; dz <= 1; dz++)
                        {
                            if ((dx, dy, dz) == (0, 0, 0))
                                continue;

                            var (x, y, z) = cube.Key;
                            var key = (x + dx, y + dy, z + dz);
                            if (space.ContainsKey(key) == false)
                            {
                                space.Add(key, '.');
                            }
                        }
                    }
                }
            }
        }

        private void DebugOutput(Dictionary<(int, int, int),char> space)
        {
            int minx = int.MaxValue, miny = int.MaxValue , minz = int.MaxValue, maxx = int.MinValue, maxy = int.MinValue, maxz = int.MinValue;
            foreach (var cube in space.Where(cube => cube.Value != '.'))
            {
                var (x, y, z) = cube.Key;
                minx = Math.Min(minx, x);
                maxx = Math.Max(maxx, x);
                miny = Math.Min(miny, y);
                maxy = Math.Max(maxy, y);
                minz = Math.Min(minz, z);
                maxz = Math.Max(maxz, z);
            }
            for (var z = minz; z <= maxz; z++)
            {
                TestContext.WriteLine($"Z={z}");
                for (var y = miny; y <= maxy; y++)
                {
                    for (var x = minx; x <= maxx; x++)
                    {
                        if (space.TryGetValue((x, y, z), out var c))
                            TestContext.Write(c.ToString());
                        else
                        {
                            TestContext.Write(".");
                        }
                    }
                    TestContext.WriteLine("");
                }
                TestContext.WriteLine("");
            }
            TestContext.WriteLine("");
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
