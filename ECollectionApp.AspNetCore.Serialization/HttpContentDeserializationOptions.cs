using ECollectionApp.AspNetCore.Serialization;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public class HttpContentDeserializationOptions
    {
        public HttpContentDeserializationOptions() => Deserializers = new List<IHttpContentSerializer>();

        public IList<IHttpContentSerializer> Deserializers { get; }

        public Func<IServiceProvider, IHttpContentSerializationHandler> Handler { get; set; }

        public HttpContentDeserializationOptions SetHandler(IHttpContentSerializationHandler handler)
        {
            Handler = (provider) => handler;
            return this;
        }
    }
}
