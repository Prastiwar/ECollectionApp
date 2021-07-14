using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ECollectionApp.WebUI.Serialization
{
    public class DefaultHttpContentDeserializationHandler : IHttpContentDeserializationHandler
    {
        public DefaultHttpContentDeserializationHandler(IEnumerable<IHttpContentDeserializer> deserializers)
            => Deserializers = new List<IHttpContentDeserializer>(deserializers);

        protected IEnumerable<IHttpContentDeserializer> Deserializers { get; }

        public async Task<T> DeserializeAsync<T>(HttpContent content)
        {
            IHttpContentDeserializer deserializer = GetDeserializer(content);
            if (deserializer == null)
            {
                throw new ArgumentNullException($"Couldn't find {typeof(IHttpContentDeserializer).Name} for this HttpContent");
            }
            return await deserializer.DeserializeAsync<T>(content);
        }

        protected IHttpContentDeserializer GetDeserializer(HttpContent content)
            => Deserializers.FirstOrDefault(x => x.CanDeserialize(content));
    }
}
