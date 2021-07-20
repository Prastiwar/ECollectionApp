using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ECollectionApp.AspNetCore.Patch
{
    public class ReflectionPatchUpdater<TModel> : PatchModelUpdater<TModel>
    {
        public override void ApplyChange(TModel toModel, KeyValuePair<string, object> change, ModelStateDictionary state)
        {
            string propertyName = change.Key;
            PropertyInfo propertyInfo = toModel.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            try
            {
                propertyInfo.SetValue(toModel, change.Value);
            }
            catch (Exception ex)
            {
                bool added = state.TryAddModelException(propertyName, ex);
                if (!added)
                {
                    throw;
                }
            }
        }
    }
}
