using System;
using System.Collections.Generic;
using System.Linq;

namespace PSnappy
{
    public static class CommonExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        public static bool HasDuplicates<T, TKey>(this IEnumerable<T> items, Func<T, TKey> keyselector)
        {
            return items.GroupBy(keyselector).Where(e => e.Count() > 1).Any();
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Comparison with modulus of 1e-5 (5 decimal places)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="comparisonvalue"></param>
        /// <returns></returns>
        public static bool IsReasonablyCloseTo(this double value, double comparisonvalue)
        {
            const double DIFF = 0.00001;
            return Math.Abs(comparisonvalue - value) < DIFF;
        }

        public static bool IsEffectivelyZero(this decimal value)
        {
            const decimal deminimis = 5;
            return value < deminimis; //if less than 5, treat it as 0
        }

        public static bool IsEffectivelyZero(this double value)
        {
            const double deminimis = 5;
            return value < deminimis; //if less than 5, treat it as 0
        }

        /// <summary>
        /// Comparison with modulus of 1e-5 (5 decimal places)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="comparisonvalue"></param>
        /// <returns></returns>
        public static bool IsReasonablyCloseToZero(this double value)
        {
            const double DIFF = 0.00001;
            return Math.Abs(value) < DIFF;
        }

        /// <summary>
        /// Comparison with modulus of 1e-9 (9 decimal places)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="comparisonvalue"></param>
        /// <returns></returns>
        public static bool IsVeryCloseTo(this double value, double comparisonvalue)
        {
            const double DIFF = 0.000000001;
            return Math.Abs(comparisonvalue - value) < DIFF;
        }

        /// <summary>
        /// Comparison with modulus of 1e-9 (9 decimal places)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="comparisonvalue"></param>
        /// <returns></returns>
        public static bool IsVeryCloseToZero(this double value)
        {
            const double DIFF = 0.000000001;
            return Math.Abs(value) < DIFF;
        }

        public static string ToYesNo(this bool value)
        {
            return value ? "Yes" : "No";
        }
    }
}
