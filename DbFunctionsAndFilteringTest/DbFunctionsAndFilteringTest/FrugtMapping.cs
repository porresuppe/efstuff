using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace DbFunctionsAndFilteringTest
{
    public class FrugtMapping : EntityTypeConfiguration<Frugt>
    {
        public FrugtMapping()
        {
            HasKey(x => x.Id);

            ToTable("Frugt");
        }
    }
}
