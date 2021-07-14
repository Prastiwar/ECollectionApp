using System.Net.Http;
using System.Threading.Tasks;

namespace ECollectionApp.WebUI.Serialization
{
    public interface IHttpContentDeserializer
    {
        bool CanDeserialize(HttpContent content);

        Task<T> DeserializeAsync<T>(HttpContent content);
    }
}
