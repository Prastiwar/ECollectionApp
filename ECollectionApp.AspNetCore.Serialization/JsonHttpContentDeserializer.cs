using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ECollectionApp.AspNetCore.Serialization
{
    public class JsonHttpContentDeserializer : IHttpContentSerializer
    {
        public bool CanDeserialize(HttpContent content) => CanSerialize(content.Headers.ContentType);

        public bool CanSerialize(MediaTypeHeaderValue mediaType) => mediaType != null &&
                                                                    (string.Compare(mediaType.MediaType, "application/json") == 0 ||
                                                                    string.Compare(mediaType.MediaType, "text/plain") == 0);

        public async Task<T> DeserializeAsync<T>(HttpContent content)
        {
            using (Stream responseStream = await content.ReadAsStreamAsync())
            {
                T value = await content.ReadFromJsonAsync<T>();
                return value;
            }
        }

        public Task<HttpContent> SerializeAsync(object value, Type valueType, MediaTypeHeaderValue mediaType)
            => Task.FromResult<HttpContent>(JsonContent.Create(value, valueType, mediaType));
    }
}
