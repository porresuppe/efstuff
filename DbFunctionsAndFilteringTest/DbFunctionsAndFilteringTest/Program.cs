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
                // SQL Server only function not part of System.Data.Entity.SqlServer.SqlFunctions
                var formatQuery = db.Fruit.Select(p => p.Created.Format("D", "pt-PT"));
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
                var patIndexQuery = db.Fruit.Select(z => z.Name).Where(z => SqlFunctions.CharIndex("b", z) > 0);
                Debug.WriteLine("Using CharIndex: " + patIndexQuery.First());

                // User defined function
                var familyCountQuery = from s in db.Fruit
                                        where MySqlFunctions.FamilyCount(s.Family) > 1
                                        select s;
                var familyCountQueryList = familyCountQuery.ToList();
                Debug.WriteLine("Using FamilyCount: " + familyCountQueryList.First().Name);

                // Simple query with filters
                var simpleQuery = db.Fruit;
                Debug.WriteLine("Filter 'IsDeleted' in use: " + simpleQuery.Select(f => f.Name).Flatten());
                db.DisableFilter("IsDeleted");
                Debug.WriteLine("Filter 'IsDeleted' disabled: " + simpleQuery.Select(f => f.Name).Flatten());
            }
        }
    }
}
