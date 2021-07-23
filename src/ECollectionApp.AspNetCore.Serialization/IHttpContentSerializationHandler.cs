using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ECollectionApp.AspNetCore.Serialization
{
    public interface IHttpContentSerializationHandler
    {
        Task<T> DeserializeAsync<T>(HttpContent content);

        Task<HttpContent> SerializeAsync(object value, Type valueType, MediaTypeHeaderValue mediaType);
    }
}
