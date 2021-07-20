using ECollectionApp.AspNetCore.Patch;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public class PatcherBuilderOptions
    {
        public Func<IServiceProvider, IChangePatcher> Patcher { get; set; }
    }
}
