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
    public class Tile
    {
        public char[][] TileMatrix { get; }

        public Tile(int tileNo, char[][] tile)
        {
            TileNo = tileNo;
            TileMatrix = tile;

            var borderTop1 = new string(TileMatrix[0]).Replace("#", "1").Replace(".", "0");
            var borderTop2 = new string(TileMatrix[0].Reverse().ToArray()).Replace("#", "1").Replace(".", "0");
            Top = (Convert.ToInt32(borderTop1, 2), Convert.ToInt32(borderTop2, 2));

            var borderBottom1 = new string(TileMatrix[TileMatrix.Length - 1]).Replace("#", "1").Replace(".", "0");
            var borderBottom2 = new string(TileMatrix[TileMatrix.Length - 1].Reverse().ToArray()).Replace("#", "1")
                .Replace(".", "0");
            Bottom = (Convert.ToInt32(borderBottom1, 2), Convert.ToInt32(borderBottom2, 2));

            var borderLeft1 = new string(TileMatrix.Select(r => r[0]).ToArray()).Replace("#", "1").Replace(".", "0");
            var borderLeft2 = new string(TileMatrix.Select(r => r[0]).Reverse().ToArray()).Replace("#", "1")
                .Replace(".", "0");
            Left = (Convert.ToInt32(borderLeft1, 2), Convert.ToInt32(borderLeft2, 2));

            var borderRight1 = new string(TileMatrix.Select(r => r[r.Length - 1]).ToArray()).Replace("#", "1")
                .Replace(".", "0");
            var borderRight2 = new string(TileMatrix.Select(r => r[r.Length - 1]).Reverse().ToArray()).Replace("#", "1")
                .Replace(".", "0");
            Right = (Convert.ToInt32(borderRight1, 2), Convert.ToInt32(borderRight2, 2));

            AllBorderPatterns = new HashSet<(int, int)>
            {
                TopNormalized,
                RightNormalized,
                BottomNormalized,
                LeftNormalized
            };
        }

        public int TileNo { get; }
        public (int, int) Top { get; set; }
        public (int, int) TopNormalized => Top.Item1 > Top.Item2 ? (Top.Item2, Top.Item1) : Top;
        public (int, int) Bottom { get; set; }
        public (int, int) BottomNormalized => Bottom.Item1 > Bottom.Item2 ? (Bottom.Item2, Bottom.Item1) : Bottom;
        public (int, int) Left { get; set; }
        public (int, int) LeftNormalized => Left.Item1 > Left.Item2 ? (Left.Item2, Left.Item1) : Left;
        public (int, int) Right { get; set; }
        public (int, int) RightNormalized => Right.Item1 > Right.Item2 ? (Right.Item2, Right.Item1) : Right;

        public ISet<(int, int)> AllBorderPatterns { get; }

        public void Rotate()
        {
            var clone = (char[][])TileMatrix.Clone();
            for (var i = 0; i < clone.Length; i++)
            {
                for (var j = 0; j < clone[i].Length; j++)
                {
                    TileMatrix[j][clone.Length - 1 - i] = clone[i][j];
                }
            }

            var temp = Top;
            Top = Left;
            Left = Bottom;
            Bottom = Right;
            Right = temp;
        }
    }

    [TestClass]
    public class Day20
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Part1()
        {
            // Iterate over lines
            var stringReader = new StringReader(DayInput);
            string line;
            var regex = new Regex(@"Tile (\d+):");
            var tiles = new Dictionary<int, Tile>();
            while ((line = stringReader.ReadLine()) != null)
            {
                var match = regex.Match(line);
                var tileNo = int.Parse(match.Groups[1].Value);

                var charsList = new List<char[]>();
                while (string.IsNullOrEmpty(line = stringReader.ReadLine()) == false)
                {
                    charsList.Add(line.ToCharArray());
                }

                var tile = new Tile(tileNo, charsList.ToArray());
                tiles.Add(tileNo, tile);
            }

            var borderCount = new Dictionary<(int, int), int>();
            foreach (var tile in tiles.Values)
            {
                foreach (var pair in new [] {tile.TopNormalized, tile.RightNormalized, tile.BottomNormalized, tile.LeftNormalized})
                {
                    if (borderCount.TryGetValue(pair, out var count))
                    {
                        borderCount[pair] = count + 1;
                    }
                    else
                    {
                        borderCount[pair] = 1;
                    }
                }
            }

            var borderCandidates = borderCount.Where(kvp => kvp.Value < 2).Select(kvp => kvp.Key).ToHashSet();

            var corners = tiles.Values.Where(t => t.AllBorderPatterns.Intersect(borderCandidates).Count() > 1).ToList();
            var product = corners.Aggregate(1L, (i, tile) => i * tile.TileNo);

            TestContext.WriteLine($"{product}");
        }

        [TestMethod]
        public void Part2()
        {
            // Iterate over lines
            var stringReader = new StringReader(TestInput);
            string line;
            var regex = new Regex(@"Tile (\d+):");
            var tiles = new Dictionary<int, Tile>();
            while ((line = stringReader.ReadLine()) != null)
            {
                var match = regex.Match(line);
                var tileNo = int.Parse(match.Groups[1].Value);

                var charsList = new List<char[]>();
                while (string.IsNullOrEmpty(line = stringReader.ReadLine()) == false)
                {
                    charsList.Add(line.ToCharArray());
                }

                var tile = new Tile(tileNo, charsList.ToArray());
                tiles.Add(tileNo, tile);
            }

            var borderCount = new Dictionary<(int, int), int>();
            foreach (var tile in tiles.Values)
            {
                foreach (var pair in new [] {tile.TopNormalized, tile.RightNormalized, tile.BottomNormalized, tile.LeftNormalized})
                {
                    if (borderCount.TryGetValue(pair, out var count))
                    {
                        borderCount[pair] = count + 1;
                    }
                    else
                    {
                        borderCount[pair] = 1;
                    }
                }
            }

            var borderCandidates = borderCount.Where(kvp => kvp.Value < 2).Select(kvp => kvp.Key).ToHashSet();

            var corners = tiles.Values.Where(t => t.AllBorderPatterns.Intersect(borderCandidates).Count() > 1).ToList();
            var borders = tiles.Values.Where(t => t.AllBorderPatterns.Intersect(borderCandidates).Count() == 1).ToList();

            var firstCorner = corners.First();
            DebugOutput(firstCorner);
            while (borderCandidates.Contains(firstCorner.RightNormalized) ||
                   borderCandidates.Contains(firstCorner.BottomNormalized))
            {
                firstCorner.Rotate();
                DebugOutput(firstCorner);
            }
        }

        private void DebugOutput(Tile tile)
        {
            for (var i = 0; i < tile.TileMatrix.Length; i++)
            {
                for (int j = 0; j < tile.TileMatrix[i].Length; j++)
                {
                    TestContext.Write(tile.TileMatrix[i][j].ToString());
                }
                TestContext.WriteLine("");
            }
            TestContext.WriteLine("");
        }


        private string TestInput = @"Tile 2311:
..##.#..#.
##..#.....
#...##..#.
####.#...#
##.##.###.
##...#.###
.#.#.#..##
..#....#..
###...#.#.
..###..###

Tile 1951:
#.##...##.
#.####...#
.....#..##
#...######
.##.#....#
.###.#####
###.##.##.
.###....#.
..#.#..#.#
#...##.#..

Tile 1171:
####...##.
#..##.#..#
##.#..#.#.
.###.####.
..###.####
.##....##.
.#...####.
#.##.####.
####..#...
.....##...

Tile 1427:
###.##.#..
.#..#.##..
.#.##.#..#
#.#.#.##.#
....#...##
...##..##.
...#.#####
.#.####.#.
..#..###.#
..##.#..#.

Tile 1489:
##.#.#....
..##...#..
.##..##...
..#...#...
#####...#.
#..#.#.#.#
...#.#.#..
##.#...##.
..##.##.##
###.##.#..

Tile 2473:
#....####.
#..#.##...
#.##..#...
######.#.#
.#...#.#.#
.#########
.###.#..#.
########.#
##...##.#.
..###.#.#.

Tile 2971:
..#.#....#
#...###...
#.#.###...
##.##..#..
.#####..##
.#..####.#
#..#.#..#.
..####.###
..#.#.###.
...#.#.#.#

Tile 2729:
...#.#.#.#
####.#....
..#.#.....
....#..#.#
.##..##.#.
.#.####...
####.#.#..
##.####...
##..#.##..
#.##...##.

Tile 3079:
#.#.#####.
.#..######
..#.......
######....
####.#..#.
.#...#.##.
#.#####.##
..#.###...
..#.......
..#.###...";

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
