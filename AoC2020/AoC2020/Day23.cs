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
    public class Day23
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Part1()
        {
            // var testInput = "389125467";
            var testInput = "925176834";

            var cups = testInput.Select(c => int.Parse(c.ToString())).ToList();
            var pos = 0;
            for (int i = 0; i < 100; i++)
            {
                // TestContext.WriteLine($"-- Move {i + 1} --");
                // DebugOutput(cups, pos);
                var pickedCups = GetPickedCups(cups, ref pos);
                // DebugOutput(pickedCups);
                var searchValue = cups[pos] - 1;
                if (searchValue < 1)
                    searchValue = cups.Max();

                int foundIndex;
                while ((foundIndex = cups.FindIndex(i => i == searchValue)) == -1)
                {
                    searchValue--;
                    if (searchValue < 1)
                        searchValue = cups.Max();
                }

                // TestContext.WriteLine($"destination: {cups[foundIndex]}");
                var destination = foundIndex + 1;
                cups.InsertRange(destination, pickedCups);
                if (destination <= pos)
                    pos += 3;

                pos = (pos + 1) % cups.Count;
            }
            DebugOutput(cups, pos);
            var start = cups.IndexOf(1);
            for (int i = (start + 1) % cups.Count; i != start ; i = (i + 1) % cups.Count)
            {
                TestContext.Write($"{cups[i]}");
            }
        }

        [TestMethod]
        public void Part2()
        {
            // var testInput = "389125467";
            var testInput = "925176834";

            // var cups = new LinkedList<int>(testInput.Select(c => int.Parse(c.ToString())));
            var cups = new LinkedList<int>(testInput.Select(c => int.Parse(c.ToString())).Concat(Enumerable.Range(10, 999991)));
            var index = new LinkedListNode<int>[1000001];
            for (var i = cups.First;  i != null; i = i.Next)
            {
                index[i.Value] = i;
            }
            TestContext.WriteLine($"{cups.Count}");
            var pos = cups.First;
            for (int i = 0; i < 10000000; i++)
            {
                // TestContext.WriteLine($"-- Move {i + 1} --");
                // DebugOutput(cups, pos);
                var pickedCups = GetPickedCups(cups, pos);
                // DebugOutput(pickedCups);
                var searchValue = pos.Value - 1;
                if (searchValue < 1)
                    searchValue = cups.Max();

                while (pickedCups.Contains(searchValue))
                {
                    searchValue--;
                    if (searchValue < 1)
                        searchValue = cups.Max();
                }

                var destination = index[searchValue];

                // TestContext.WriteLine($"destination: {destination.Value}");
                while (pickedCups.Count > 0)
                {
                    var last = pickedCups.Last;
                    pickedCups.RemoveLast();
                    cups.AddAfter(destination, last);
                }

                pos = pos.Next ?? cups.First;
            }
            var start = cups.Find(1);
            if (start == null)
                throw new Exception("Didn't find 1");
            var next = start.Next ?? cups.First;
            long firstValue = next.Value;
            TestContext.WriteLine($"{firstValue}");
            next = next.Next ?? cups.First;
            long secondValue = next.Value;
            TestContext.WriteLine($"{secondValue}");
            TestContext.WriteLine($"Product: {firstValue * secondValue}");
        }

        private LinkedList<int> GetPickedCups(LinkedList<int> cups, LinkedListNode<int> pos)
        {
            var pickedCups = new LinkedList<int>();
            var pickPos = pos.Next;
            while (pickedCups.Count < 3)
            {
                if (pickPos == null)
                    pickPos = cups.First;
                var nextPos = pickPos.Next;
                cups.Remove(pickPos);
                pickedCups.AddLast(pickPos);
                pickPos = nextPos;
            }

            return pickedCups;
        }

        private static List<int> GetPickedCups(List<int> cups, ref int pos)
        {
            if (pos + 3 < cups.Count)
            {
                var pickedCups = cups.GetRange(pos + 1, 3);
                cups.RemoveRange(pos + 1, 3);
                return pickedCups;
            }
            else
            {
                var pickedCups = cups.GetRange(pos + 1, cups.Count - pos - 1);
                cups.RemoveRange(pos + 1, cups.Count - pos - 1);
                var remainder = 3 - pickedCups.Count;
                pickedCups.AddRange(cups.GetRange(0, remainder));
                cups.RemoveRange(0, remainder);
                pos = pos - remainder;
                return pickedCups;
            }
        }

        private void DebugOutput(LinkedList<int> cups, LinkedListNode<int> pos = null)
        {
            if (pos == null)
                TestContext.Write($"pick up: ");
            else
                TestContext.Write($"cups: ");
            foreach (var cup in cups)
            {
                TestContext.Write(cup == pos?.Value ? $"({cup}) " : $"{cup} ");
            }
            TestContext.WriteLine("");
        }

        private void DebugOutput(List<int> cups, int pos = -1)
        {
            if (pos == -1)
                TestContext.Write($"pick up: ");
            else
                TestContext.Write($"cups: ");
            for (int i = 0; i < cups.Count; i++)
            {
                if (i == pos)
                    TestContext.Write($"({cups[i]}) ");
                else
                    TestContext.Write($"{cups[i]} ");
            }
            TestContext.WriteLine("");
        }
    }
}
