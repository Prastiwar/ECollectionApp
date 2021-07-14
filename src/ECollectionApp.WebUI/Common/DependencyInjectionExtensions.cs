using ECollectionApp.WebUI.Serialization;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddHttpContentDeserializationHandler(this IServiceCollection services, Action<HttpContentDeserializationOptions> options = null)
        {
            HttpContentDeserializationOptions builder = new HttpContentDeserializationOptions() {
                Handler = provider => new DefaultHttpContentDeserializationHandler(provider.GetServices<IHttpContentDeserializer>())
            };
            builder.Deserializers.Add(new JsonHttpContentDeserializer());
            options?.Invoke(builder);

            services.AddSingleton(builder.Handler);
            foreach (IHttpContentDeserializer deserializer in builder.Deserializers)
            {
                services.AddTransient(provider => deserializer);
            }
        }
    }
}
