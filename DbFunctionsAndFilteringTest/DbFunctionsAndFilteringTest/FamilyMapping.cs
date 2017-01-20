using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace DbFunctionsAndFilteringTest
{
    public class FamilyMapping : EntityTypeConfiguration<Family>
    {
        public FamilyMapping()
        {
            HasKey(x => x.Id);

            ToTable("Family");
        }
    }
}
