﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zcarey_Advent_of_Code_2023
{
    internal class Day11 : AdventOfCodeProblem
    {
        public object Part1(string input)
        {
            (Space[][] map, bool[] emptyRows, bool[] emptyCols, Point[] galaxies) = ParseInput(input);

            Func<Point, Point, int> galaxyDistanceFunc = (Point galaxy1, Point galaxy2) =>
            {
                // Check horizontal distance
                int x1 = Math.Min(galaxy1.X, galaxy2.X);
                int x2 = Math.Max(galaxy1.X, galaxy2.X);
                int dx = x2 - x1; 
                for (int x = x1 + 1; x <= x2 - 1; x++)
                {
                    if (emptyCols[x] == true)
                    {
                        dx++;
                    }
                }

                // Check vertical distance
                int y1 = Math.Min(galaxy1.Y, galaxy2.Y);
                int y2 = Math.Max(galaxy1.Y, galaxy2.Y);
                int dy = y2 - y1;
                for (int y = y1 + 1; y <= y2 - 1; y++)
                {
                    if (emptyRows[y] == true)
                    {
                        dy++;
                    }
                }

                return dx + dy;
            };

            return galaxies
                .UniquePairs()
                .Select(x => galaxyDistanceFunc(x.Pair1, x.Pair2))
                .Sum();
        }

        public object Part2(string input)
        {
            return "";
        }

        struct Space
        {
            public bool IsEmpty => !IsGalaxy;
            public bool IsGalaxy = false;

            public Space() 
            { 
            
            }
        }

        (Space[][] Map, bool[] EmptyRows, bool[] EmptyCols, Point[] GalaxyLocations) ParseInput(string input)
        {
            List<Point> galaxies = new();
            string[] lines = input.GetLines().ToArray();
            Space[][] map = new Space[lines.Length][];
            for(int y = 0; y < lines.Length; y++)
            {
                map[y] = new Space[lines[y].Length];
                for (int x = 0; x < lines[y].Length; x++)
                {
                    map[y][x].IsGalaxy = (lines[y][x] == '#');
                    if (map[y][x].IsGalaxy)
                    {
                        galaxies.Add(new Point(x, y));
                    }
                }
            }

            // Find empty rows
            bool[] emptyRows = new bool[map.Length];
            for (int y = 0; y < map.Length; y++)
            {
                emptyRows[y] = true;
                for (int x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x].IsGalaxy)
                    {
                        emptyRows[y] = false;
                        break;
                    }
                }
            }

            // Find empty columns
            bool[] emptyCols = new bool[map[0].Length];
            for (int x = 0; x < map[0].Length; x++)
            {
                emptyCols[x] = true;
                for (int y = 0; y < map.Length; y++)
                {
                    if (map[y][x].IsGalaxy)
                    {
                        emptyCols[x] = false;
                        break;
                    }
                }
            }

            return (map, emptyRows, emptyCols, galaxies.ToArray());
        }
    }
}
