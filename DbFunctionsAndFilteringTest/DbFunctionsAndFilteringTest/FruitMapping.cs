﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace DbFunctionsAndFilteringTest
{
    public class FruitMapping : EntityTypeConfiguration<Fruit>
    {
        public FruitMapping()
        {
            HasKey(x => x.Id);

            HasRequired(f => f.Family).WithMany(fam => fam.Fruits).Map(fk => fk.MapKey("FamilyId"));

            ToTable("Fruit");
        }
    }
}
