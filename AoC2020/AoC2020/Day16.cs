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
    public class Day16
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Part1()
        {
            // Iterate over lines
            var stringReader = new StringReader(DayInput);
            string line;
            var rules = new Dictionary<string, (int, int, int, int)>();
            var regex = new Regex(@"(.+): (\d+)-(\d+) or (\d+)-(\d+)");
            while ((line = stringReader.ReadLine()) != "")
            {
                var match = regex.Match(line);
                var groups = match.Groups;
                rules.Add(groups[1].Value,
                    (int.Parse(groups[2].Value), int.Parse(groups[3].Value), int.Parse(groups[4].Value),
                        int.Parse(groups[5].Value)));
            }

            while ((line = stringReader.ReadLine()) != "nearby tickets:")
                ;

            var tickets = new List<int[]>();
            while ((line = stringReader.ReadLine()) != null)
            {
                tickets.Add(line.Split(',').Select(int.Parse).ToArray());
            }

            var sum = 0;
            ISet<int[]> invalidTickets = new HashSet<int[]>();
            foreach (var ticket in tickets)
            {
                foreach (var field in ticket)
                {
                    var valid = false;
                    foreach (var rule in rules)
                    {
                        var (lb1, ub1, lb2, ub2) = rule.Value;
                        if ((field < lb1 || field > ub1) && (field < lb2 || field > ub2))
                            continue;

                        valid = true;
                    }

                    if (!valid)
                    {
                        sum += field;
                        invalidTickets.Add(ticket);
                    }
                }
            }

            TestContext.WriteLine($"{sum}");
        }

        [TestMethod]
        public void Part2()
        {
            // Iterate over lines
            var stringReader = new StringReader(DayInput);
            string line;
            var rules = new Dictionary<string, (int, int, int, int)>();
            var regex = new Regex(@"(.+): (\d+)-(\d+) or (\d+)-(\d+)");
            while ((line = stringReader.ReadLine()) != "")
            {
                var match = regex.Match(line);
                var groups = match.Groups;
                rules.Add(groups[1].Value, (int.Parse(groups[2].Value), int.Parse(groups[3].Value), int.Parse(groups[4].Value), int.Parse(groups[5].Value)));
            }


            while ((line = stringReader.ReadLine()) != "your ticket:") { }

            line = stringReader.ReadLine();
            var myTicket = line.Split(',').Select(int.Parse).ToArray();

            while ((line = stringReader.ReadLine()) != "nearby tickets:") { }

            var tickets = GetValidTickets(stringReader, rules).ToArray();

            var fields = new Dictionary<string, ISet<int>>();
            foreach (var rule in rules)
            {
                for (var field = 0; field < tickets[0].Length; field++)
                {
                    var valid = true;
                    foreach (var ticket in tickets)
                    {
                        var (lb1, ub1, lb2, ub2) = rule.Value;
                        var currentField = ticket[field];
                        if (currentField >= lb1 && currentField <= ub1 || currentField >= lb2 && currentField <= ub2)
                            continue;

                        valid = false;
                    }

                    if (!valid)
                        continue;

                    if (fields.TryGetValue(rule.Key, out var set))
                    {
                        set.Add(field);
                    }
                    else
                    {
                        fields.Add(rule.Key, new HashSet<int> {field});
                    }
                }
            }

            var knownFields = new HashSet<int>();
            foreach (var field in fields.OrderBy(f => f.Value.Count))
            {
                field.Value.ExceptWith(knownFields);
                if (field.Value.Count > 1)
                    throw new Exception("Unspecified field");
                knownFields.Add(field.Value.First());
            }

            var finalFields = fields.ToDictionary(k => k.Key, v => v.Value.Single());
            var product = 1L;
            foreach (var kvp in finalFields.Where(k => k.Key.StartsWith("departure")))
            {
                product *= myTicket[kvp.Value];
            }

            TestContext.WriteLine($"{product}");
        }

        private static IEnumerable<int[]> GetValidTickets(StringReader stringReader, Dictionary<string, (int, int, int, int)> rules)
        {
            string line;
            while ((line = stringReader.ReadLine()) != null)
            {
                var ticket = line.Split(',').Select(int.Parse).ToArray();
                var validTicket = true;
                foreach (var field in ticket)
                {
                    var valid = false;
                    foreach (var rule in rules)
                    {
                        var (lb1, ub1, lb2, ub2) = rule.Value;
                        if ((field < lb1 || field > ub1) && (field < lb2 || field > ub2))
                            continue;

                        valid = true;
                    }

                    if (!valid)
                    {
                        validTicket = false;
                        break;
                    }
                }

                if (validTicket)
                    yield return ticket;
            }
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
