using System.Data.Entity.Core.Metadata.Edm;

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

        public virtual DbSet<Frugt> Frugt { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            AddConfigurations(modelBuilder);
            AddConventions(modelBuilder);
        }

        private static void AddConfigurations(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new FrugtMapping());
        }

        private static void AddConventions(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(new FormatFunctionConvention());
            modelBuilder.Conventions.Add(new FamilieCountFunctionConvention());
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

    public class FamilieCountFunctionConvention : AbstractStoreModelConvention
    {
        public override void Apply(EdmModel item, DbModel model)
        {
            var valueParameter = FunctionParameter.Create("familie", this.GetStorePrimitiveType(model, PrimitiveTypeKind.String), ParameterMode.In);
            var returnValue = FunctionParameter.Create("result", this.GetStorePrimitiveType(model, PrimitiveTypeKind.Int32), ParameterMode.ReturnValue);
            CreateAndAddFunction(item, "FamilieCount", new[] { valueParameter }, new[] { returnValue });
        }
    }
}
