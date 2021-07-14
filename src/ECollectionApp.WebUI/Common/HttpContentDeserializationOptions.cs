using ECollectionApp.WebUI.Serialization;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public class HttpContentDeserializationOptions
    {
        public HttpContentDeserializationOptions() => Deserializers = new List<IHttpContentDeserializer>();

        public IList<IHttpContentDeserializer> Deserializers { get; }

        public Func<IServiceProvider, IHttpContentDeserializationHandler> Handler { get; set; }

        public HttpContentDeserializationOptions SetHandler(IHttpContentDeserializationHandler handler)
        {
            Handler = (provider) => handler;
            return this;
        }
    }
}
