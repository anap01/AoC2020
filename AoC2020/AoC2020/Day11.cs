using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2020
{
    [TestClass]
    public class Day11
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Part1()
        {
            // Iterate over lines
            var stringReader = new StringReader(DayInput);
            string line;
            var seats = new Dictionary<(int, int), char>();
            var row = 0;
            while ((line = stringReader.ReadLine()) != null)
            {
                foreach (var (c, col) in line.Select((c, i) => (c, i)))
                {
                    if (c == 'L')
                        seats.Add((row, col), 'L');
                }

                row++;
            }

            var actions = new List<Action>();
            do
            {
                actions.ForEach(a => a());
                actions.Clear();

                foreach (var seat in seats)
                {
                    if (ToggleSeat(seat.Key, seat.Value, seats, out var action))
                    {
                        actions.Add(action);
                    }
                }

            } while (actions.Count > 0);

            TestContext.WriteLine($"{seats.Values.Count(c => c == '#')}");
        }

        private static bool ToggleSeat((int, int) key, char c, IDictionary<(int, int), char> seats, out Action action)
        {
            action = null;
            var (row, col) = key;
            var deltas = new[] {-1, 0, 1};
            var occupied = 0;
            foreach (var dx in deltas)
            {
                foreach (var dy in deltas)
                {
                    if ((dx, dy) == (0, 0))
                        continue;

                    if (seats.TryGetValue((row + dy, col + dx), out var cc))
                        occupied += cc == '#' ? 1 : 0;
                }
            }

            switch (c)
            {
                case '#':
                    if (occupied >= 4)
                    {
                        action = () => seats[(row, col)] = 'L';
                        return true;
                    }
                    break;
                case 'L':
                    if (occupied == 0)
                    {
                        action = () => seats[(row, col)] = '#';
                        return true;
                    }
                    break;
            }

            return false;
        }

        [TestMethod]
        public void Part2()
        {
            var stringReader = new StringReader(DayInput);

            string line;
            var seats = new Dictionary<(int, int), char>();
            var row = 0;
            while ((line = stringReader.ReadLine()) != null)
            {
                foreach (var (c, col) in line.Select((c, i) => (c, i)))
                {
                    if (c == 'L')
                        seats.Add((row, col), 'L');
                }

                row++;
            }

            var actions = new List<Action>();
            do
            {
                actions.ForEach(a => a());
                actions.Clear();

                foreach (var seat in seats)
                {
                    if (ToggleSeat2(seat.Key, seat.Value, seats, out var action))
                    {
                        actions.Add(action);
                    }
                }

            } while (actions.Count > 0);

            TestContext.WriteLine($"{seats.Values.Count(c => c == '#')}");
        }

        private static bool ToggleSeat2((int, int) key, char c, IDictionary<(int, int), char> seats, out Action action)
        {
            action = null;
            var (row, col) = key;
            var maxRow = 0;
            var maxCol = 0;
            foreach (var (rr, cc) in seats.Keys)
            {
                maxRow = Math.Max(rr, maxRow);
                maxCol = Math.Max(cc, maxCol);
            }
            var deltas = new[] {(0, -1), (-1, -1), (-1, 0), (-1, 1), (0, 1), (1, 1), (1, 0), (1, -1)};
            var occupied = 0;
            foreach (var (dx, dy) in deltas)
            {
                var testRow = row;
                var testCol = col;

                do
                {
                    testRow += dy;
                    testCol += dx;
                    if (seats.TryGetValue((testRow, testCol), out var cc))
                    {
                        occupied += cc == '#' ? 1 : 0;
                        break;
                    }
                } while (testRow >= 0 && testRow <= maxRow && testCol >= 0 && testCol <= maxCol);
            }

            switch (c)
            {
                case '#':
                    if (occupied >= 5)
                    {
                        action = () => seats[(row, col)] = 'L';
                        return true;
                    }
                    break;
                case 'L':
                    if (occupied == 0)
                    {
                        action = () => seats[(row, col)] = '#';
                        return true;
                    }
                    break;
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
