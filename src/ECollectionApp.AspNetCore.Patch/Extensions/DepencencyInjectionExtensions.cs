using ECollectionApp.AspNetCore.Patch;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DepencencyInjectionExtensions
    {
        public static void AddChangePatcher(this IServiceCollection services, Action<PatcherBuilderOptions> options = null)
        {
            PatcherBuilderOptions builderOptions = new PatcherBuilderOptions() {
                Patcher = provider => new DefaultChangePatcher(provider)
            };
            options?.Invoke(builderOptions);
            if (builderOptions.Patcher == null)
            {
                throw new ArgumentNullException(nameof(builderOptions.Patcher));
            }
            services.AddSingleton(builderOptions.Patcher);
        }
    }
}
