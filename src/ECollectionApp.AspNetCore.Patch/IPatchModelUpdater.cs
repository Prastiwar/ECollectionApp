using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace ECollectionApp.AspNetCore.Patch
{
    public interface IPatchModelUpdater
    {
        bool CanUpdate(object model);

        void ApplyChange(object toModel, KeyValuePair<string, object> change, ModelStateDictionary state);
    }

    public interface IPatchModelUpdater<TModel> : IPatchModelUpdater
    {
        void ApplyChange(TModel toModel, KeyValuePair<string, object> change, ModelStateDictionary state);
    }
}
