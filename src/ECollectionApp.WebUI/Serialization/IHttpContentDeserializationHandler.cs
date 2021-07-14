using System.Net.Http;
using System.Threading.Tasks;

namespace ECollectionApp.WebUI.Serialization
{
    public interface IHttpContentDeserializationHandler
    {
        Task<T> DeserializeAsync<T>(HttpContent content);
    }
}
