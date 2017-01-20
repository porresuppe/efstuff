using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbFunctionsAndFilteringTest
{
    public static class StringExtensions
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
    }
}