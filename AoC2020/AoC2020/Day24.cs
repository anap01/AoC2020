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
    public class Day24
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Part1()
        {
            // Iterate over lines
            var stringReader = new StringReader(DayInput);
            string line;
            var blackTiles = new HashSet<(int, int)>();
            while ((line = stringReader.ReadLine()) != null)
            {
                var pos = (x: 0, y: 0);
                var diag = false;
                foreach (var c in line)
                {
                    switch (c)
                    {
                        case 'e':
                            pos = (pos.x + (diag ? 1 : 2), pos.y);
                            diag = false;
                            break;
                        case 'w':
                            pos = (pos.x - (diag ? 1 : 2), pos.y);
                            diag = false;
                            break;
                        case 'n':
                            pos = (pos.x, pos.y + 1);
                            diag = true;
                            break;
                        case 's':
                            pos = (pos.x, pos.y - 1);
                            diag = true;
                            break;
                    }
                }

                if (blackTiles.Add(pos) == false)
                    blackTiles.Remove(pos);
            }

            TestContext.WriteLine($"{blackTiles.Count}");
        }

        [TestMethod]
        public void Part2()
        {
            // Iterate over lines
            var stringReader = new StringReader(DayInput);
            string line;
            var blackTiles = new HashSet<(int, int)>();
            while ((line = stringReader.ReadLine()) != null)
            {
                var pos = (x: 0, y: 0);
                var diag = false;
                foreach (var c in line)
                {
                    switch (c)
                    {
                        case 'e':
                            pos = (pos.x + (diag ? 1 : 2), pos.y);
                            diag = false;
                            break;
                        case 'w':
                            pos = (pos.x - (diag ? 1 : 2), pos.y);
                            diag = false;
                            break;
                        case 'n':
                            pos = (pos.x, pos.y + 1);
                            diag = true;
                            break;
                        case 's':
                            pos = (pos.x, pos.y - 1);
                            diag = true;
                            break;
                    }
                }

                if (blackTiles.Add(pos) == false)
                    blackTiles.Remove(pos);
            }

            for (int i = 0; i < 100; i++)
            {
                Flip(blackTiles);
            }

            TestContext.WriteLine($"{blackTiles.Count}");
        }

        private void Flip(HashSet<(int, int)> blackTiles)
        {
            var flips = new List<Action>();
            var whiteTileNeighbours = new HashSet<(int, int)>();
            foreach (var blackTile in blackTiles)
            {
                var neighbours = Neighbours(blackTile, blackTiles, whiteTileNeighbours);
                if (neighbours == 0 || neighbours > 2)
                    flips.Add(() => blackTiles.Remove(blackTile));
            }

            foreach (var whiteTileNeighbour in whiteTileNeighbours)
            {
                var neighbours = Neighbours(whiteTileNeighbour, blackTiles);
                if (neighbours == 2)
                    flips.Add(() => blackTiles.Add(whiteTileNeighbour));
            }
            flips.ForEach(f => f.Invoke());
        }

        private int Neighbours((int, int) blackTile, ICollection<(int, int)> blackTiles, ISet<(int, int)> whiteNeighbours = null)
        {
            var deltas = new[] {(-2, 0), (2, 0), (-1, 1), (1, 1), (-1, -1), (1, -1)};
            int neighbours =  0;
            foreach (var delta in deltas)
            {
                var testpos = (blackTile.Item1 + delta.Item1, blackTile.Item2 + delta.Item2);
                if (blackTiles.Contains(testpos))
                    neighbours++;
                else
                    whiteNeighbours?.Add(testpos);
            }

            return neighbours;
        }

        private string TestInput = @"sesenwnenenewseeswwswswwnenewsewsw
neeenesenwnwwswnenewnwwsewnenwseswesw
seswneswswsenwwnwse
nwnwneseeswswnenewneswwnewseswneseene
swweswneswnenwsewnwneneseenw
eesenwseswswnenwswnwnwsewwnwsene
sewnenenenesenwsewnenwwwse
wenwwweseeeweswwwnwwe
wsweesenenewnwwnwsenewsenwwsesesenwne
neeswseenwwswnwswswnw
nenwswwsewswnenenewsenwsenwnesesenew
enewnwewneswsewnwswenweswnenwsenwsw
sweneswneswneneenwnewenewwneswswnese
swwesenesewenwneswnwwneseswwne
enesenwswwswneneswsenwnewswseenwsese
wnwnesenesenenwwnenwsewesewsesesew
nenewswnwewswnenesenwnesewesw
eneswnwswnwsenenwnwnwwseeswneewsenese
neswnwewnwnwseenwseesewsenwsweewe
wseweeenwnesenwwwswnew";

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
