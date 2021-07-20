using Microsoft.AspNetCore.Mvc.ModelBinding;
using ECollectionApp.AspNetCore.Patch;
using System;
using System.Collections.Generic;

namespace ECollectionApp.CollectionService.Data
{
    public class CollectionPatchUpdater : PatchModelUpdater<Collection>
    {
        public override void ApplyChange(Collection toModel, KeyValuePair<string, object> change, ModelStateDictionary state)
        {
            try
            {
                ApplyChange(toModel, change, state);
            }
            catch (Exception ex)
            {
                bool added = state.TryAddModelException(change.Key, ex);
                if (!added)
                {
                    throw;
                }
            }
        }

        protected virtual void ApplyChange(Collection toModel, KeyValuePair<string, object> change)
        {
            switch (change.Key)
            {
                case nameof(Collection.GroupId):
                    toModel.GroupId = Convert.ToInt32(change.Value);
                    break;
                case nameof(Collection.Name):
                    toModel.Name = change.Value.ToString();
                    break;
                case nameof(Collection.Value):
                    toModel.Value = change.Value.ToString();
                    break;
                default:
                    throw new InvalidOperationException($"Cannot change path '{change.Key}'");
            }
        }
    }
}
