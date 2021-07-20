using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECollectionApp.AspNetCore.Patch
{
    public class DefaultChangePatcher : IChangePatcher
    {
        public DefaultChangePatcher(IServiceProvider provider) => Provider = provider;

        protected IServiceProvider Provider { get; }

        public void ApplyTo<T>(T model, PatchDocument document, ModelStateDictionary state)
        {
            IEnumerable<IPatchModelUpdater> updaters = Provider.GetServices<IPatchModelUpdater>();
            IPatchModelUpdater updater = updaters.FirstOrDefault(x => x.CanUpdate(model));
            if (updater == null)
            {
                throw new KeyNotFoundException($"Cannot find {nameof(IPatchModelUpdater)} for type of {model.GetType()}");
            }
            foreach (KeyValuePair<string, object> change in document)
            {
                updater.ApplyChange(model, change, state);
            }
        }
    }
}
