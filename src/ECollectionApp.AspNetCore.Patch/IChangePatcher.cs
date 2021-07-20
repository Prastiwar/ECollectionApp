using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ECollectionApp.AspNetCore.Patch
{
    public interface IChangePatcher
    {
        void ApplyTo<T>(T model, PatchDocument document, ModelStateDictionary state);
    }
}
