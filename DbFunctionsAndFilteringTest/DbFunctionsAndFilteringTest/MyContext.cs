using System;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using EntityFramework.DynamicFilters;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Collections.Generic;

namespace DbFunctionsAndFilteringTest
{
    public class MyContext : DbContext
    {
        public MyContext()
            : base("name=MyContext")
        {
        }

        public virtual DbSet<Family> Families { get; set; }
        public virtual DbSet<Fruit> Fruits { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            AddConfigurations(modelBuilder);
            AddConventions(modelBuilder);
            AddFilters(modelBuilder);
        }

        private static void AddConfigurations(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new FamilyMapping());
            modelBuilder.Configurations.Add(new FruitMapping());
        }

        private static void AddConventions(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(new FormatFunctionConvention());
            modelBuilder.Conventions.Add(new FruitCountFunctionConvention());
        }

        private static void AddFilters(DbModelBuilder modelBuilder)
        {
            modelBuilder.Filter("IsDeleted", (IDeleted d) => d.Deleted, false);
            modelBuilder.Filter("SingleFamily", (Fruit f, string famName) => f.Family.Name == famName, "");
            var values = new List<string> { "Banan", "Ananas" };
            modelBuilder.Filter("ContainsFruit", (Fruit f, List<string> valueList) => valueList.Contains(f.Name), () => values);
        }
    }

    public class FormatFunctionConvention : AbstractStoreModelConvention
    {
        public FormatFunctionConvention()
        {
            IsBuiltIn = true;
        }

        public override void Apply(EdmModel item, DbModel model)
        {
            var valueParameter = FunctionParameter.Create("value", this.GetStorePrimitiveType(model, PrimitiveTypeKind.DateTime), ParameterMode.In);
            var formatParameter = FunctionParameter.Create("format", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.In);
            var cultureParameter = FunctionParameter.Create("culture", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.In);
            var returnValue = FunctionParameter.Create("result", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.ReturnValue);
            CreateAndAddFunction(item, "FORMAT", new[] { valueParameter, formatParameter, cultureParameter }, new[] { returnValue });
        }
    }

    public class FruitCountFunctionConvention : AbstractStoreModelConvention
    {
        public override void Apply(EdmModel item, DbModel model)
        {
            var valueParameter = FunctionParameter.Create("family", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.In);
            var returnValue = FunctionParameter.Create("result", this.GetStorePrimitiveType(model, PrimitiveTypeKind.Int32), ParameterMode.ReturnValue);
            CreateAndAddFunction(item, "FruitCount", new[] { valueParameter }, new[] { returnValue });
        }
    }
}
