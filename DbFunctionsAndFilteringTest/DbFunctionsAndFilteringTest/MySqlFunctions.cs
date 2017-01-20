using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbFunctionsAndFilteringTest
{
    public static class MySqlFunctions
    {
        [DbFunction("CodeFirstDatabaseSchema", "FORMAT")]
        public static String Format(this DateTime value, String format, String culture)
        {
            return (value.ToString(format, CultureInfo.CreateSpecificCulture(culture)));
        }

        [DbFunction("CodeFirstDatabaseSchema", "FamilyCount")]
        public static int? FamilyCount(string family)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }
    }
}
