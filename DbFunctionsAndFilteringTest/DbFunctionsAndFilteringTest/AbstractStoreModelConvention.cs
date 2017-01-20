using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace DbFunctionsAndFilteringTest
{
    public abstract class AbstractStoreModelConvention : IStoreModelConvention<EdmModel>
    {
        protected AbstractStoreModelConvention()
        {
            IsBuiltIn = false;
        }

        protected bool IsBuiltIn { get; set; }

        public abstract void Apply(EdmModel item, DbModel model);

        protected EdmFunction CreateAndAddFunction(EdmModel item, string name, IList<FunctionParameter> parameters, IList<FunctionParameter> returnValues)
        {
            var payload = new EdmFunctionPayload
            {
                StoreFunctionName = name, 
                Parameters = parameters, 
                ReturnParameters = returnValues, 
                Schema = GetDefaultSchema(item),
                IsBuiltIn = IsBuiltIn
            };
            var function = EdmFunction.Create(name, GetDefaultNamespace(item), item.DataSpace, payload, null);
            item.AddItem(function);
            return (function);
        }

        protected EdmType GetStorePrimitiveType(DbModel model, PrimitiveTypeKind typeKind)
        {
            return (model.ProviderManifest.GetStoreType(TypeUsage.CreateDefaultTypeUsage(PrimitiveType.GetEdmPrimitiveType(typeKind))).EdmType);
        }

        protected string GetDefaultNamespace(EdmModel layerModel)
        {
            return (layerModel.GlobalItems.OfType<EdmType>().Select(t => t.NamespaceName).Distinct().Single());
        }

        protected string GetDefaultSchema(EdmModel layerModel)
        {
            return (layerModel.Container.EntitySets.Select(s => s.Schema).Distinct().SingleOrDefault());
        }
    }
}