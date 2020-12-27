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
    public class Day25
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Part1()
        {
            var publicKeys = DayInput.Split(new [] { '\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
            // var cardPublicKey = 5764801L;
            // var doorPublicKey = 17807724L;
            var cardPublicKey = long.Parse(publicKeys[0]);
            var doorPublicKey = long.Parse(publicKeys[1]);

            var cardLoopSize = 1L;
            while (cardPublicKey != BigInteger.ModPow(7, cardLoopSize, 20201227))
                cardLoopSize++;

            var doorLoopSize = 1L;
            while (doorPublicKey != BigInteger.ModPow(7, doorLoopSize, 20201227))
                doorLoopSize++;


            TestContext.WriteLine($"Card loop size: {cardLoopSize}");
            TestContext.WriteLine($"Door loop size: {doorLoopSize}");

            var encryptionKey = BigInteger.ModPow(cardPublicKey, doorLoopSize, 20201227);
            TestContext.WriteLine($"Encryption key: {encryptionKey}");

        }

        private long Transform2(long subjectNumber, long loopSize)
        {
            return (long) (Math.Pow(subjectNumber, loopSize) % 20201227);
        }
        private long Transform(long subjectNumber, long loopSize)
        {
            var transform = 1L;
            for (long i = 0; i < loopSize; i++)
            {
                transform *= subjectNumber;
                transform %= 20201227;
            }
            return transform;
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
