using System.Data.Entity.SqlServer;
using System.Diagnostics;
using System.Linq;
using EntityFramework.DynamicFilters;

namespace DbFunctionsAndFilteringTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new MyContext())
            {
                db.DisableFilter("OnlyMusaceaeFamily");

                // SQL Server only function not part of System.Data.Entity.SqlServer.SqlFunctions
                var formatQuery = db.Fruits.Select(p => p.Created.Format("D", "pt-PT"));
                try
                {
                    formatQuery.ToList();
                }
                catch
                {
                    // ignored
                    // No FORMAT in SQL Server 2008
                }

                // SQL Server only function
                var patIndexQuery = db.Fruits.Select(z => z.Name).Where(z => SqlFunctions.CharIndex("b", z) > 0);
                Debug.WriteLine("Using CharIndex: " + patIndexQuery.First());

                // User defined function
                var familyCountQuery = from s in db.Families.GetMultipleIncluding(x => x.Fruits)
                                       where MySqlFunctions.FruitCount(s.Name) > 1
                                       select s;
                var familyCountQueryList = familyCountQuery.ToList();
                Debug.WriteLine("Using FruitCount: " + familyCountQueryList.First().Name);

                // Simple queries with filters
                var simpleQuery = db.Fruits;
                Debug.WriteLine("Filter 'IsDeleted' in use: " + simpleQuery.Select(f => f.Name).Flatten());
                db.SetFilterScopedParameterValue("IsDeleted", true);
                Debug.WriteLine("Deleted rows: " + simpleQuery.Select(f => f.Name).Flatten());
                db.DisableFilter("IsDeleted");
                Debug.WriteLine("Filter 'IsDeleted' disabled: " + simpleQuery.Select(f => f.Name).Flatten());
                db.EnableFilter("OnlyMusaceaeFamily");
                Debug.WriteLine("Filter 'OnlyMusaceaeFamily' in use: " + simpleQuery.Select(f => f.Name).Flatten());
            }
        }
    }
}
