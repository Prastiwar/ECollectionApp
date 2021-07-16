using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ECollectionApp.WebUI.Serialization
{
    public class DefaultHttpContentSerializationHandler : IHttpContentSerializationHandler
    {
        public DefaultHttpContentSerializationHandler(IEnumerable<IHttpContentSerializer> serializers)
            => Serializers = new List<IHttpContentSerializer>(serializers);

        protected IEnumerable<IHttpContentSerializer> Serializers { get; }

        public Task<T> DeserializeAsync<T>(HttpContent content)
        {
            IHttpContentSerializer serializer = GetDeserializer(content);
            if (serializer == null)
            {
                throw new ArgumentNullException($"Couldn't find {typeof(IHttpContentSerializer).Name} for this HttpContent");
            }
            return serializer.DeserializeAsync<T>(content);
        }

        public Task<HttpContent> SerializeAsync(object value, Type valueType, MediaTypeHeaderValue mediaType)
        {
            IHttpContentSerializer serializer = GetSerializer(mediaType);
            if (serializer == null)
            {
                throw new ArgumentNullException($"Couldn't find {typeof(IHttpContentSerializer).Name} for this HttpContent");
            }
            return serializer.SerializeAsync(value, valueType ?? value.GetType(), mediaType);
        }

        protected IHttpContentSerializer GetDeserializer(HttpContent content)
            => Serializers.FirstOrDefault(x => x.CanDeserialize(content));

        protected IHttpContentSerializer GetSerializer(MediaTypeHeaderValue mediaType)
            => Serializers.FirstOrDefault(x => x.CanSerialize(mediaType));
    }
}
