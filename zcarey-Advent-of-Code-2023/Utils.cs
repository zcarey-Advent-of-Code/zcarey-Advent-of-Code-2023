using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zcarey_Advent_of_Code_2023
{
    public static class Utils
    {

        public static IEnumerable<string> GetLines(this StringReader reader)
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }
        }

        public static IEnumerable<string> GetLines(this string str)
        {
            StringReader reader = new StringReader(str);
            return reader.GetLines();
        }

        public static void Increment<T>(this Dictionary<T, int> dict, T key, int inc)
        {
            if (!dict.ContainsKey(key))
            {
                dict[key] = inc;
            } else
            {
                dict[key] += inc;
            }
        }

        public static IEnumerable<(int Index, T Element)> WithIndex<T>(this IEnumerable<T> elements)
        {
            int index = 0;
            foreach (var element in elements)
            {
                yield return (index, element);
                index++;
            }
        }

        public static IEnumerable<T> Repeat<T>(this IEnumerable<T> elements)
        {
            while(true)
            {
                foreach(T element in elements)
                {
                    yield return element;
                }
            }
        }

        public static long GCF(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public static long LCM(long a, long b)
        {
            return (a / GCF(a, b)) * b;
        }

        public static long LCM(IEnumerable<long> elements)
        {
            bool first = true;
            long lcm = 0;
            foreach(long n in elements)
            {
                if (first)
                {
                    first = false;
                    lcm = n;
                } else
                {
                    lcm = LCM(n, lcm);
                }
            }

            return lcm;
        }
    }

    struct LargeRange
    {
        public long Start;
        public long End;

        public LargeRange() { }

        public LargeRange(long start, long end)
        {
            this.Start = start;
            this.End = end;
        }

        public static LargeRange FromLength(long start, long length)
        {
            return new LargeRange(start, start + length - 1);
        }
    }
}
