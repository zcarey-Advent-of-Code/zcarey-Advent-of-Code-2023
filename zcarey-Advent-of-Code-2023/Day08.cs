using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zcarey_Advent_of_Code_2023
{
    internal class Day08 : AdventOfCodeProblem
    {
        public object Part1(string input)
        {
            (var turns, var nodes) = ParseInput(input);
            string currentNode = "AAA";
            int count = 0;
            IEnumerator<Direction> nextTurn = turns.GetEnumerator();
            while (currentNode != "ZZZ")
            {
                nextTurn.MoveNext();
                if (nextTurn.Current == Direction.Left)
                {
                    currentNode = nodes[currentNode].left;
                } else
                {
                    currentNode = nodes[currentNode].right;
                }

                count++;
            }

            return count;
        }

        public object Part2(string input)
        {
            return "";
        }

        enum Direction
        {
            Left,
            Right
        }

        (IEnumerable<Direction> turns, Dictionary<string, (string left, string right)> nodes) ParseInput(string input)
        {
            Direction[] turns = input.GetLines().First().Select(c => (c == 'L') ? Direction.Left : Direction.Right).ToArray();
            Dictionary<string, (string left, string right)> nodes = new();

            foreach(string line in input.GetLines().Skip(2))
            {
                string key = line.Substring(0, 3);
                string left = line.Substring(7, 3);
                string right = line.Substring(12, 3);

                nodes.Add(key, (left, right));
            }

            return (turns.Repeat(), nodes);
        }

    }
}
