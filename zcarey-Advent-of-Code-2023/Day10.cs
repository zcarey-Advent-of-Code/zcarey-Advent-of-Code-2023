﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zcarey_Advent_of_Code_2023
{
    internal class Day10 : AdventOfCodeProblem
    {
        public object Part1(string input)
        {
            (Point Start, Pipe[][] Map) = GetMap(input);
            return Follow(Map, Start).Count() / 2;
        }

        public object Part2(string input)
        {
            return "";
        }

        public enum Rotation
        {
            North = 0,
            South = 2,
            East = 1, 
            West = 3
        }
/*
        static Rotation Clockwise(Rotation rot)
        {
            return (Rotation)(((int)rot + 1) % 4);
        }

        static Rotation CounterClockwise(Rotation rot)
        {
            if ((int)rot == 0)
            {
                return (Rotation)3;
            } else
            {
                return (Rotation)((int)rot - 1);
            }
        }
*/
        struct Pipe
        {
            public char c;
            public bool IsGround => c == '.';
            public bool IsStart => c == 'S';
            public (int DX, int DY, Rotation NewRotation) GetDelta(Rotation context)
            {
                switch (c)
                {
                    case '|': return (0, context == Rotation.North ? -1 : 1, context);
                    case '-': return (context == Rotation.West ? -1 : 1, 0, context);
                    case 'L': return (context == Rotation.South) ? (1, 0, Rotation.East) : (0, -1, Rotation.North);
                    case 'J': return (context == Rotation.South) ? (-1, 0, Rotation.West) : (0, -1, Rotation.North);
                    case '7': return (context == Rotation.East) ? (0, 1, Rotation.South) : (-1, 0, Rotation.West);
                    case 'F': return (context == Rotation.West) ? (0, 1, Rotation.South) : (1, 0, Rotation.East);
                    default: throw new Exception();
                }
            }
            public bool ConnectsNorth => c == '|' || c == 'L' || c == 'J';
            public bool ConnectsEast => c == '-' || c == 'L' || c == 'F';
            public bool ConnectSouth => c == '|' || c == '7' || c == 'F';
            public bool ConnectWest => c == '-' || c == 'J' || c == '7';

            public Pipe(char c)
            {
                this.c = c;
            }
        }

        (Point Start, Pipe[][] Map) GetMap(string input)
        {
            string[] lines = input.GetLines().ToArray();
            int width = lines[0].Length;
            int height = lines[1].Length;

            Point start = new();
            Pipe[][] map = new Pipe[height][];
            for (int y = 0; y < height; y++)
            {
                map[y] = new Pipe[width]; ;
                for (int x = 0; x < width; x++)
                {
                    map[y][x] = new Pipe(lines[y][x]);

                    if (map[y][x].IsStart)
                    {
                        start = new Point(x, y);
                    }
                }
            }

            // Replace the start with the correct pipe
            if ((start.Y > 0) && (map[start.Y - 1][start.X].ConnectSouth))
            { // Connection to north
                if ((start.X < width - 1) && (map[start.Y][start.X + 1].ConnectWest))
                { // Connection to east
                    map[start.Y][start.X] = new Pipe('L');
                }
                else if ((start.Y < height - 1) && (map[start.Y + 1][start.X].ConnectsNorth))
                { // Connection to south
                    map[start.Y][start.X] = new Pipe('|');
                }
                else if ((start.X > 0) && (map[start.Y][start.X - 1].ConnectsEast))
                { // Connection to west
                    map[start.Y][start.X] = new Pipe('J');
                }
            }
            else if ((start.X < width - 1) && (map[start.Y][start.X + 1].ConnectWest))
            { // Connection to east
                if ((start.Y < height - 1) && (map[start.Y + 1][start.X].ConnectsNorth))
                { // Connection to south
                    map[start.Y][start.X] = new Pipe('F');
                }
                else if ((start.X > 0) && (map[start.Y][start.X - 1].ConnectsEast))
                { // Connection to west
                    map[start.Y][start.X] = new Pipe('-');
                }
                else
                {
                    throw new Exception();
                }
            }
            else if ((start.Y < height - 1) && (map[start.Y + 1][start.X].ConnectsNorth))
            { // Connection to south
                if ((start.X > 0) && (map[start.Y][start.X - 1].ConnectsEast))
                { // Connection to west
                    map[start.Y][start.X] = new Pipe('7');
                }
                else
                {
                    throw new Exception();
                }
            }
            else
            {
                throw new Exception();
            }

            return (start, map);
        }

        IEnumerable<Pipe> Follow(Pipe[][] map, Point start)
        {
            // Includes start position as last path
            int width = map[0].Length;
            int height = map.Length;
            Pipe startPipe = map[start.Y][start.X];

            // Pick a starting direction
            Rotation rotation;
            if (startPipe.ConnectsNorth)
            {
                rotation = Rotation.South;
            } else if (startPipe.ConnectsEast)
            {
                rotation = Rotation.West;
            } else if (startPipe.ConnectSouth)
            {
                rotation = Rotation.North;
            } else
            {
                rotation = Rotation.East;
            }

            int x = start.X;
            int y = start.Y;
            do
            {
                var delta = map[y][x].GetDelta(rotation);
                x += delta.DX;
                y += delta.DY;
                rotation = delta.NewRotation;
                yield return map[y][x];
            } while (x != start.X || y != start.Y);
        }
    }
}
