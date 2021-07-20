using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace ECollectionApp.AspNetCore.Patch
{
    public abstract class PatchModelUpdater<TModel> : IPatchModelUpdater<TModel>
    {
        public abstract void ApplyChange(TModel toModel, KeyValuePair<string, object> change, ModelStateDictionary state);

        public bool CanUpdate(object model) => model is TModel;

        void IPatchModelUpdater.ApplyChange(object toModel, KeyValuePair<string, object> change, ModelStateDictionary state) => ApplyChange((TModel)toModel, change, state);
    }
}
