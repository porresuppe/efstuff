using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbFunctionsAndFilteringTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new MyContext())
            {
                // Simple query
                var simpleQuery = db.Frugt.FirstOrDefault(x => x.Id == 1);

                // SQL Server only function not part of System.Data.Entity.SqlServer.SqlFunctions
                var formatQuery = db.Frugt.Select(p => p.Oprettet.Format("D", "pt-PT"));
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
                var patIndexQuery = db.Frugt.Select(z => z.Navn).Where(z => SqlFunctions.PatIndex(z, "%o%a%") > 0);

                // User defined function
                var familieCountQuery = from s in db.Frugt
                                        where MySqlFunctions.FamilieCount(s.Familie) > 1
                                        select s;
                var familieCountQueryList = familieCountQuery.ToList();
            }
        }
    }
}
