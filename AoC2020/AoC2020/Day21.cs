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
    public class Day21
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Part1and2()
        {
            // Iterate over lines
            var stringReader = new StringReader(DayInput);
            string line;
            var regex = new Regex(@"(?<ingredients>.*)\(contains (?<allergens>.*)\)");
            var allergens = new Dictionary<string, HashSet<string>>();
            var foods = new List<HashSet<string>>();
            while ((line = stringReader.ReadLine()) != null)
            {
                var match = regex.Match(line);
                var ingredients = match.Groups["ingredients"].Value.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries).ToHashSet();
                foods.Add(ingredients);
                foreach (var allergen in match.Groups["allergens"].Value.Split(new string[] {", "}, StringSplitOptions.None))
                {
                    if (allergens.TryGetValue(allergen, out var currentIngredients))
                    {
                        currentIngredients.IntersectWith(ingredients);
                    }
                    else
                    {
                        allergens[allergen] = ingredients.ToHashSet();
                    }
                }
            }

            var groupedAllergens = allergens.GroupBy(kvp => kvp.Value.Count == 1).ToList();
            var definedAllergens = groupedAllergens.Where(k => k.Key == true).SelectMany(g => g).ToList();
            var overdefinedALlergens = groupedAllergens.Where(k => k.Key == false).SelectMany(g => g).ToList();
            var initialCount = int.MaxValue;
            while (overdefinedALlergens.Count < initialCount)
            {
                initialCount = overdefinedALlergens.Count;
                for (var i = overdefinedALlergens.Count - 1; i >= 0; i--)
                {
                    foreach (var definedAllergen in definedAllergens.ToList())
                    {
                        overdefinedALlergens[i].Value.ExceptWith(definedAllergen.Value);
                        if (overdefinedALlergens[i].Value.Count == 1)
                        {
                            definedAllergens.Add(overdefinedALlergens[i]);
                            overdefinedALlergens.RemoveAt(i);
                        }
                    }
                }

            }
            var sum = 0;
            foreach (var food in foods)
            {
                foreach (var allergenIngredients in allergens.Values)
                {
                    food.ExceptWith(allergenIngredients);
                }

                sum += food.Count;
            }
            TestContext.WriteLine($"{sum}");
            var cdil = string.Join(",", allergens.OrderBy(kvp => kvp.Key).SelectMany(kvp => kvp.Value));
            TestContext.WriteLine(cdil);
        }

        private string TestInput = @"mxmxvkd kfcds sqjhc nhms (contains dairy, fish)
trh fvjkl sbzzf mxmxvkd (contains dairy)
sqjhc fvjkl (contains soy)
sqjhc mxmxvkd sbzzf (contains fish)";

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
