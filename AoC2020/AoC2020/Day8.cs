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
    public class Day8
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Day8Part1()
        {
            var strings = DayInput.Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);
            var acc = 0;
            var p = 0;
            var visited = new HashSet<int>();
            while (true)
            {
                if (visited.Add(p) == false)
                    break;

                var inst = strings[p].Substring(0, 3);
                var val = strings[p].Substring(4);
                switch (strings[p].Substring(0, 3))
                {
                    case "acc":
                        acc += int.Parse(val);
                        p++;
                        break;
                    case "jmp":
                        p += int.Parse(val);
                        break;
                    case "nop":
                        p++;
                        break;
                }
            }
            TestContext.WriteLine($"{acc}");
        }

        [TestMethod]
        public void Day8Part2()
        {
            var strings = DayInput.Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);
            var nops = new List<int>();
            var jmps = new List<int>();
            for (int i = 0; i < strings.Length; i++)
            {
                if (strings[i].StartsWith("nop"))
                    nops.Add(i);
                if (strings[i].StartsWith("jmp"))
                    jmps.Add(i);
            }

            int acc;
            foreach (var nop in nops)
            {
                strings[nop] = strings[nop].Replace("nop", "jmp");
                if (Run(strings, out acc))
                {
                    TestContext.WriteLine($"{acc}");
                    return;
                }
                strings[nop] = strings[nop].Replace("jmp", "nop");
            }
            foreach (var jmp in jmps)
            {
                strings[jmp] = strings[jmp].Replace("jmp", "nop");
                if (Run(strings, out acc))
                {
                    TestContext.WriteLine($"{acc}");
                    return;
                }
                strings[jmp] = strings[jmp].Replace("nop", "jmp");
            }
            do
            {

            } while (Run(strings, out acc) == false);

        }

        private static bool Run(string[] strings, out int acc)
        {
            var p = 0;
            acc = 0;
            var visited = new HashSet<int>();
            while (true)
            {
                if (visited.Add(p) == false)
                    break;

                var inst = strings[p].Substring(0, 3);
                var val = strings[p].Substring(4);
                switch (strings[p].Substring(0, 3))
                {
                    case "acc":
                        acc += int.Parse(val);
                        p++;
                        break;
                    case "jmp":
                        p += int.Parse(val);
                        break;
                    case "nop":
                        p++;
                        break;
                }

                if (p == strings.Length)
                    break;
            }

            return p == strings.Length;
        }

        private string DayInput
        {
            get
            {
                using var client = new WebClient();
                client.Headers.Add(HttpRequestHeader.Cookie, Environment.GetEnvironmentVariable("sessionCookie"));
                return client.DownloadString($"https://adventofcode.com/2020/day/{this.GetType().Name.Substring(3)}/input");
            }
        }
    }
}
