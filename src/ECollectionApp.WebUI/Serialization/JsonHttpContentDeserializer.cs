using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECollectionApp.WebUI.Serialization
{
    public class JsonHttpContentDeserializer : IHttpContentDeserializer
    {
        public bool CanDeserialize(HttpContent content) => string.Compare(content.Headers.ContentType.MediaType, "application/json") == 0;

        public async Task<T> DeserializeAsync<T>(HttpContent content)
        {
            using (Stream responseStream = await content.ReadAsStreamAsync())
            {
                return await JsonSerializer.DeserializeAsync<T>(responseStream);
            }
        }
    }
}
