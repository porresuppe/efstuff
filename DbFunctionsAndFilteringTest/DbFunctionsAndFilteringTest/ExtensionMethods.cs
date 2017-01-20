using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DbFunctionsAndFilteringTest
{
    public static class ExtensionMethods
    {
        public static string Flatten(this IEnumerable<string> listOfStrings, string seperator = ", ")
        {
            var ofStrings = listOfStrings as string[] ?? listOfStrings.ToArray();
            
            if (listOfStrings == null || !ofStrings.Any())
                return string.Empty;

            var result = new StringBuilder();

            foreach (var s in ofStrings)
            {
                result.AppendFormat("{0}{1}", s, seperator);
            }
            result.Remove(result.Length - seperator.Length, seperator.Length);
            
            return result.ToString();
        }

        public static IQueryable<T> GetMultipleIncluding<T>(this IQueryable<T> query, params Expression<Func<T, object>>[] includes) where T : class
        {
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return query;
        }
    }
}