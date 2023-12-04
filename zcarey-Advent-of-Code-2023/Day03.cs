using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zcarey_Advent_of_Code_2023
{
    internal class Day03 : AdventOfCodeProblem
    {
        public object Part1(string input)
        {
            return IdentifyPartNumbers(input).Sum();
        }

        public object Part2(string input)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<int> IdentifyPartNumbers(string input)
        {
            HashSet<int> partNumberIDs = new HashSet<int>();
            List<int> numbers = new List<int>(); // Stored numbers for later use
            List<List<int>> schematic = new List<List<int>>(); // Our self-expanding map

            int y = -1; // Current height in map
            foreach(string line in input.GetLines())
            {
                y++;
                List<int> currentMap = new List<int>();
                schematic.Add(currentMap);

                for(int x = 0; x < line.Length; x++)
                {
                    char c = line[x];
                    if (c == '.')
                    {
                        currentMap.Add(-1);
                    } else if (char.IsDigit(c))
                    {
                        int startX = x;
                        // Read the number
                        int number = 0;
                        int numberID = numbers.Count() + 1;
                        while (x < line.Length && char.IsDigit(c = line[x]))
                        {
                            number *= 10;
                            number += (c - '0');
                            currentMap.Add(numberID);
                            x++;
                        }
                        // x is now 1 past the number (possible out of bounds), move back one so when the loop continues it will check this char
                        x--;
                        numbers.Add(number);

                        // Check for previous symbols
                        bool previousSymbol = false;
                        if (startX >= 1 && currentMap[startX - 1] == 0)
                        {
                            previousSymbol = true;
                        }
                        else if (y > 0)
                        {
                            List<int> prevMap = schematic[y - 1];
                            for (int i = startX - 1; i <= x + 1; i++)
                            {
                                if (i >= 0 && i < prevMap.Count && prevMap[i] == 0)
                                {
                                    previousSymbol = true;
                                    break;
                                }
                            }
                        }
                        if (previousSymbol)
                        {
                            partNumberIDs.Add(numberID);
                        }
                    } else
                    {
                        // Must be a symbol
                        currentMap.Add(0);

                        // Check for previous numbers
                        if (x >= 1 && currentMap[x - 1] > 0)
                        {
                            int numberID = currentMap[x - 1];
                            partNumberIDs.Add(numberID);
                        } 
                        if (y > 0)
                        {
                            List<int> prevMap = schematic[y - 1];
                            for (int i = x - 1; i <= x + 1; i++)
                            {
                                if (i >= 0 && i < prevMap.Count && prevMap[i] > 0)
                                {
                                    partNumberIDs.Add(prevMap[i]);
                                }
                            }
                        }
                    }
                }
            }

            foreach(int ID in partNumberIDs)
            {
                yield return numbers[ID - 1];
            }
        }
    }
}
