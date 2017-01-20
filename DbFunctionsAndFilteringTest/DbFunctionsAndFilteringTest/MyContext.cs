using System.Data.Entity.Core.Metadata.Edm;
using EntityFramework.DynamicFilters;

namespace DbFunctionsAndFilteringTest
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public class MyContext : DbContext
    {
        public MyContext()
            : base("name=MyContext")
        {
        }

        public virtual DbSet<Fruit> Fruit { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            AddConfigurations(modelBuilder);
            AddConventions(modelBuilder);
            AddFilters(modelBuilder);
        }

        private static void AddConfigurations(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new FruitMapping());
        }

        private static void AddConventions(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(new FormatFunctionConvention());
            modelBuilder.Conventions.Add(new FamilyCountFunctionConvention());
        }

        private void AddFilters(DbModelBuilder modelBuilder)
        {
            modelBuilder.Filter("IsDeleted", (IDeleted d) => d.Deleted, false);
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

    public class FamilyCountFunctionConvention : AbstractStoreModelConvention
    {
        public override void Apply(EdmModel item, DbModel model)
        {
            var valueParameter = FunctionParameter.Create("family", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.In);
            var returnValue = FunctionParameter.Create("result", this.GetStorePrimitiveType(model, PrimitiveTypeKind.Int32), ParameterMode.ReturnValue);
            CreateAndAddFunction(item, "FamilyCount", new[] { valueParameter }, new[] { returnValue });
        }
    }
}
