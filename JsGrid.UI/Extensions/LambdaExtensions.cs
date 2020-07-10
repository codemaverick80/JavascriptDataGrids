using System;
using System.Collections.Generic;
using System.Linq;

namespace JsDataGrids.UI.Extensions
{
    public static class LambdaExtensions
    {
        public static IEnumerable<TKey> DistinctByColumn<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector)
        {
            return source.GroupBy(selector).Select(x => x.Key);
        }


        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> property)
        {
            return source.GroupBy(property).Select(x => x.First());
        }

    }
}