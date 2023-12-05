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

    }
}
