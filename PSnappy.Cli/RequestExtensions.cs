using System;
using System.Collections.Generic;
using System.Linq;

namespace PSnappy.Cli
{
    public static class RequestExtensions
    {
        public static string RequiredString(this string s, string argName)
        {
            var conformed = s.ConformString();

            if (conformed == null)
            {
                throw new Exception($"{argName} is required!");
            }

            return conformed;
        }

        public static string ConformString(this string s, string defaultValue = null)
        {
            return string.IsNullOrWhiteSpace(s) ? defaultValue : s.Trim();
        }

        public static IEnumerable<string> Conform(this IEnumerable<string> items)
        {
            if (items.Count() != 1)
            {
                return items;
            }

            var item = items.First();

            if (item.Contains(","))
            {
                // split it out -- they forgot a space
                return item.Split(',');
            }

            return items;
        }
    }
}
