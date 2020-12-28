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
    public class Day22
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Part1()
        {
            // Iterate over lines
            var stringReader = new StringReader(DayInput);
            string line;
            var player1 = new Queue<int>();
            var player2 = new Queue<int>();
            var currentDeck = player1;
            while ((line = stringReader.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line))
                    currentDeck = player2;
                if (int.TryParse(line, out var card) == false)
                    continue;
                currentDeck.Enqueue(card);
            }

            while (player1.Count != 0 && player2.Count != 0)
            {
                var card1 = player1.Dequeue();
                var card2 = player2.Dequeue();
                if (card1 > card2)
                {
                    player1.Enqueue(card1);
                    player1.Enqueue(card2);
                }
                else
                {
                    player2.Enqueue(card2);
                    player2.Enqueue(card1);
                }
            }


            if (player1.Count > 0)
                currentDeck = player1;
            else
                currentDeck = player2;

            var sum = currentDeck.Select((c, i) => (c, currentDeck.Count - i)).Sum(t => t.c * t.Item2);
            TestContext.WriteLine($"{sum}");
        }

        [TestMethod]
        public void Part2()
        {
            // Iterate over lines
            var stringReader = new StringReader(DayInput);
            string line;
            var player1 = new Queue<int>();
            var player2 = new Queue<int>();
            var currentDeck = player1;
            while ((line = stringReader.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line))
                    currentDeck = player2;
                if (int.TryParse(line, out var card) == false)
                    continue;
                currentDeck.Enqueue(card);
            }


            PlayGame(player1, player2);


            if (player1.Count > 0)
                currentDeck = player1;
            else
                currentDeck = player2;

            var sum = currentDeck.Select((c, i) => (c, currentDeck.Count - i)).Sum(t => t.c * t.Item2);
            TestContext.WriteLine($"{sum}");
        }

        private static int PlayGame(Queue<int> player1, Queue<int> player2)
        {
            var previousRounds = new HashSet<string>();
            while (player1.Count != 0 && player2.Count != 0)
            {
                if (previousRounds.Add(ToStringRepresentation(player1, player2)) == false)
                {
                    return 1;
                }
                var card1 = player1.Dequeue();
                var card2 = player2.Dequeue();
                if (player1.Count >= card1 && player2.Count >= card2)
                {
                    var winner = PlayGame(new Queue<int>(player1.Take(card1)), new Queue<int>(player2.Take(card2)));
                    if (winner == 1)
                    {
                        player1.Enqueue(card1);
                        player1.Enqueue(card2);
                    }
                    else
                    {
                        player2.Enqueue(card2);
                        player2.Enqueue(card1);
                    }
                }
                else if (card1 > card2)
                {
                    player1.Enqueue(card1);
                    player1.Enqueue(card2);
                }
                else
                {
                    player2.Enqueue(card2);
                    player2.Enqueue(card1);
                }
            }

            return player1.Count > 0 ? 1 : 2;
        }

        private static string ToStringRepresentation(IEnumerable<int> player1, IEnumerable<int> player2)
        {
            var stringBuilder = new StringBuilder("Player 1:");
            stringBuilder.AppendLine();
            foreach (var card in player1)
            {
                stringBuilder.AppendLine($"{card}");
            }
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Player 2:");
            foreach (var card in player2)
            {
                stringBuilder.AppendLine($"{card}");
            }

            return stringBuilder.ToString();
        }

        private void DebugOutput(IEnumerable<int> deck)
        {
            foreach (var card in deck)
            {
                TestContext.WriteLine(card.ToString());
            }
        }

        private string TestInput2 = @"Player 1:
43
19

Player 2:
2
29
14";

        private string TestInput = @"Player 1:
9
2
6
3
1

Player 2:
5
8
4
7
10";

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
