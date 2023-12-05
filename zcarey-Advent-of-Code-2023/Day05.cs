using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zcarey_Advent_of_Code_2023
{
    internal class Day05 : AdventOfCodeProblem
    {
        public object Part1(string input)
        {
            (Almanac almanac, List<long> seeds) = ParseInput(input);
            foreach(Converter converter in almanac.Converters)
            {
                for(int i = 0; i < seeds.Count; i++)
                {
                    long seed = seeds[i];
                    long soil = converter.Convert(seed);
                    seeds[i] = soil;
                }
            }

            return seeds.Min();
        }

        public object Part2(string input)
        {
            return "";
        }

        static (Almanac, List<long>) ParseInput(string input)
        {
            List<long> seeds = new();
            Almanac almanac = new Almanac();

            IEnumerator<string> iterator = input.GetLines().GetEnumerator();
            while(iterator.MoveNext())
            {
                string line = iterator.Current;
                if (line.StartsWith("seeds:"))
                {
                    seeds.AddRange(line.Substring("seeds: ".Length).Split().Select(long.Parse));
                } else if (line == "seed-to-soil map:")
                {
                    almanac.SeedToSoil = Converter.Parse(iterator);
                } else if (line == "soil-to-fertilizer map:")
                {
                    almanac.SoilToFertilizer = Converter.Parse(iterator);
                } else if (line == "fertilizer-to-water map:")
                {
                    almanac.FertilizerToWater = Converter.Parse(iterator);
                } else if (line == "water-to-light map:")
                {
                    almanac.WaterToLight = Converter.Parse(iterator);
                } else if (line == "light-to-temperature map:")
                {
                    almanac.LightToTemperature = Converter.Parse(iterator);
                } else if (line == "temperature-to-humidity map:")
                {
                    almanac.TemperatureToHumidity = Converter.Parse(iterator);
                } else if (line == "humidity-to-location map:")
                {
                    almanac.HumidityToLocation = Converter.Parse(iterator);
                }
            }

            return (almanac, seeds);
        }

        struct ConverterRange
        {
            public long SrcRangeStart;
            public long SrcRangeEnd;
            public long DestRangeStart;
            public long DestRangeEnd;

            public bool TryConvert(long input, out long result)
            {
                if (input >= SrcRangeStart && input <= SrcRangeEnd)
                {
                    result = (input - SrcRangeStart) + DestRangeStart;
                    return true;
                } else
                {
                    result = default;
                    return false;
                }
            }

            public static ConverterRange Parse(string input)
            {
                ConverterRange range = new();
                long[] values = input.Split().Select(long.Parse).ToArray();
                range.DestRangeStart = values[0];
                range.SrcRangeStart = values[1];
                range.DestRangeEnd = values[0] + values[2];
                range.SrcRangeEnd = values[1] + values[2];
                return range;
            }
        }

        struct Converter
        {
            List<ConverterRange> ranges = new();
            public Converter() { }

            public long Convert(long input)
            {
                long result = default;
                bool converted = false;
                foreach(ConverterRange range in ranges)
                {
                    if (range.TryConvert(input, out result))
                    {
                        converted = true;
                        break;
                    }
                }

                if (converted)
                {
                    return result;
                } else
                {
                    return input;
                }
            }

            public static Converter Parse(IEnumerator<string> input)
            {
                Converter converter = new();
                while (input.MoveNext())
                {
                    string line = input.Current;
                    if (line.Length == 0)
                    {
                        break;
                    }
                    converter.ranges.Add(ConverterRange.Parse(line));
                }

                return converter;
            }
        }

        struct Almanac
        {
            public Converter[] Converters = new Converter[7];
            public ref Converter SeedToSoil => ref Converters[0];
            public ref Converter SoilToFertilizer => ref Converters[1];
            public ref Converter FertilizerToWater => ref Converters[2];
            public ref Converter WaterToLight => ref Converters[3];
            public ref Converter LightToTemperature => ref Converters[4];
            public ref Converter TemperatureToHumidity => ref Converters[5];
            public ref Converter HumidityToLocation => ref Converters[6];
            public Almanac() { }
        }
    }

}
