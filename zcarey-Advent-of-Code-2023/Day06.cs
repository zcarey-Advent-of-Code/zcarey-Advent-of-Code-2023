using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zcarey_Advent_of_Code_2023
{
    internal class Day06 : AdventOfCodeProblem
    {
        public object Part1(string input)
        {
            return ParseInput(input) // Get the race info
                .Select(x => BeatRecord(x.Distance, x.Time)) // Determine what values we need to beat the record
                .Select(x => x.End - x.Start + 1) // Calculate the margine (AKA number of possible values)
                .Aggregate((x, y) => x * y); // Multiply all the values together
        }

        LargeRange BeatRecord(int record, int time)
        {
            // The equation for how far the boat travels is d = -(x^2) + T*x
            // where T is the length of the competition, and x is the length the button is held.
            // Using this equation, we can find the range of which the button needs to be pressed
            // in order to defeat the record by solving for x (quadratic formmula).
            // x1 = -0.5*T + sqrt(T^2 - 4*d)/-2
            // x2 = -0.5*T - sqrt(T^2 - 4*d)/-2
            double rhs = Math.Sqrt(Math.Pow(time, 2) - 4.0 * record) / -2.0;

            const double tieBreaker = 0.0001; // If the result comes out an EXACT integer, it means we would tie, but we want to win!!!
            int x1 = (int)Math.Ceiling(0.5 * time + rhs + tieBreaker);
            int x2 = (int)Math.Floor(0.5 * time - rhs - tieBreaker);
            return new LargeRange(x1, x2);
        }


        public object Part2(string input)
        {
            return "";
        }

        struct Race
        {
            public int Time;
            public int Distance;
        }

        IEnumerable<Race> ParseInput(string input)
        {
            string[] lines = input.GetLines().ToArray();
            int[] time = lines[0].Substring("Time:".Length)
                .Trim()
                .Split()
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(int.Parse)
                .ToArray();
            int[] dist = lines[1].Substring("Distance:".Length)
                .Trim()
                .Split()
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(int.Parse)
                .ToArray();

            for (int i = 0; i < time.Length; i++)
            {
                Race race;
                race.Time = time[i];
                race.Distance = dist[i];
                yield return race;
            }
        }
    }
}
