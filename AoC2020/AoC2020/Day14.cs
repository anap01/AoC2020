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
    public class Day14
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Part1()
        {
            // Iterate over lines
            var stringReader = new StringReader(DayInput);
            string line;
            var regex = new Regex(@"mem\[(\d+)\] = (\d+)");
            var mem = new Dictionary<int, long>();
            var mask = new char[0];
            while ((line = stringReader.ReadLine()) != null)
            {
                if (line.StartsWith("mask"))
                {
                    mask = line.Substring(7).ToArray();
                    continue;
                }

                var match = regex.Match(line);
                var value = long.Parse(match.Groups[2].Value);
                var address = int.Parse(match.Groups[1].Value);
                foreach (var (bit, index) in mask.Select((m, i) => (m, i)))
                {
                    switch (bit)
                    {
                        case 'X':
                            continue;
                        case '1':
                            value |= 1L << (35 - index);
                            break;
                        case '0':
                            var currentMask = 0b1111_1111_1111_1111_1111_1111_1111_1111_1111;
                            currentMask ^= 1L << (35 - index);
                            value &= currentMask;
                            break;
                    }
                }

                mem[address] = value;
            }

            TestContext.WriteLine($"{mem.Values.Sum()}");
        }

        [TestMethod]
        public void Part2()
        {
            // Iterate over lines
            var stringReader = new StringReader(DayInput);
            string line;
            var regex = new Regex(@"mem\[(\d+)\] = (\d+)");
            var mem = new Dictionary<long, long>();
            var mask = new char[0];
            while ((line = stringReader.ReadLine()) != null)
            {
                if (line.StartsWith("mask"))
                {
                    mask = line.Substring(7).ToArray();
                    continue;
                }

                var match = regex.Match(line);
                var value = long.Parse(match.Groups[2].Value);
                long address = int.Parse(match.Groups[1].Value);
                var xpos = new List<int>();
                foreach (var (bit, index) in mask.Select((m, i) => (m, i)))
                {
                    switch (bit)
                    {
                        case '0':
                            continue;
                        case '1':
                            address |= 1L << (35 - index);
                            break;
                        case 'X':
                            xpos.Add(35 - index);
                            break;
                    }
                }

                var addresses = new List<long>();
                var limit = Math.Pow(2, xpos.Count);
                for (var i = 0; i < limit; i++)
                {
                    var bitString = Convert.ToString(i, 2).PadLeft(xpos.Count, '0');
                    bitString = bitString.Substring(Math.Max(bitString.Length - xpos.Count, 0), xpos.Count);
                    var newAddress = address;
                    foreach (var (bit, index) in bitString.Select((s, ii) => (s, ii)))
                    {
                        var pos = xpos[index];
                        switch (bit)
                        {
                            case '0':
                                var currentMask = 1L << pos;
                                currentMask = ~currentMask;
                                newAddress &= currentMask;
                                break;
                            case '1':
                                newAddress |= 1L << pos;
                                break;
                        }
                    }

                    addresses.Add(newAddress);
                }

                foreach (var address2 in addresses)
                {
                    mem[address2] = value;
                }
            }

            TestContext.WriteLine($"{mem.Values.Sum()}");
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
