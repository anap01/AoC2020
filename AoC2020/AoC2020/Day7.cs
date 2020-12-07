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
    [DebuggerDisplay("{" + nameof(Color) + "} ({Bags.Count})")]
    public class Bag
    {
        public Bag(string color)
        {
            Color = color;
        }

        public string Color { get; set; }
        public IList<Bag> Bags { get; } = new List<Bag>();
    }

    [TestClass]
    public class Day7
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Day7Part1()
        {
            var bags = ReadBags();
            var carriers = GetCarriers(bags, bags["shiny gold"]);
            var count = carriers.Distinct().Count();
            TestContext.WriteLine($"{count}");
        }

        [TestMethod]
        public void Day7Part2()
        {
            var bags = ReadBags();
            var carriers = GetContent(bags, bags["shiny gold"]);
            var count = carriers.Count();
            TestContext.WriteLine($"{count}");
        }

        private IEnumerable<Bag> GetContent(Dictionary<string,Bag> bags, Bag bag)
        {
            foreach (var childBag in bag.Bags)
            {
                yield return childBag;
                foreach (var childBagBag in GetContent(bags, childBag))
                {
                    yield return childBagBag;
                }
            }
        }

        private Dictionary<string, Bag> ReadBags()
        {
            // Iterate over lines
            var stringReader = new StringReader(Day7Input);
            string line;

            var bags = new Dictionary<string, Bag>();
            var regex1 = new Regex(@"(.*) bags contain (.*)");
            var regex2 = new Regex(@"(\d+) (.*?) bags?");
            while ((line = stringReader.ReadLine()) != null)
            {
                var matches1 = regex1.Match(line);
                var color = matches1.Groups[1].Value;
                if (!bags.TryGetValue(color, out var parentBag))
                {
                    parentBag = new Bag(color);
                    bags.Add(color, parentBag);
                }

                var matches2 = regex2.Matches(matches1.Groups[2].Value);
                foreach (Match match in matches2)
                {
                    var childColor = match.Groups[2].Value;
                    if (!bags.TryGetValue(childColor, out var childBag))
                    {
                        childBag = new Bag(childColor);
                        bags.Add(childColor, childBag);
                    }

                    for (int i = 0; i < int.Parse(match.Groups[1].Value); i++)
                    {
                        parentBag.Bags.Add(childBag);
                    }
                }
            }

            return bags;
        }

        private IEnumerable<Bag> GetCarriers(Dictionary<string,Bag> bags, Bag bag)
        {
            foreach (var parentBag in bags.Values.Where(b => b.Bags.Contains(bag)))
            {
                yield return parentBag;

                foreach (var carrier in GetCarriers(bags, parentBag))
                {
                    yield return carrier;
                }
            }
        }


        private string Day7Input
        {
            get
            {
                using var client = new WebClient();
                client.Headers.Add(HttpRequestHeader.Cookie, Environment.GetEnvironmentVariable("sessionCookie"));
                return client.DownloadString("https://adventofcode.com/2020/day/7/input");
            }
        }
    }
}
