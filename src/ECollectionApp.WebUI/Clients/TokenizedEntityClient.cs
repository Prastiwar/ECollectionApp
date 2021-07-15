using ECollectionApp.WebUI.Serialization;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ECollectionApp.WebUI.Clients
{
    public abstract class TokenizedEntityClient<TClient> : ITokenizedClient<TClient>
        where TClient : ITokenizedClient<TClient>
    {
        public TokenizedEntityClient(HttpClient client, IHttpContentDeserializationHandler deserializer)
        {
            Client = client;
            Deserializer = deserializer;
        }

        protected HttpClient Client { get; }

        protected IHttpContentDeserializationHandler Deserializer { get; }

        protected string Token { get; set; }

        protected abstract TClient ReturnClient();

        protected Task<HttpResponseMessage> SendwithToken(HttpMethod method, string url, bool ensureSuccess = true)
            => SendwithToken(new HttpRequestMessage(method, url), ensureSuccess);

        protected async Task<HttpResponseMessage> SendwithToken(HttpRequestMessage request, bool ensureSuccess = true)
        {
            string scheme = Client.DefaultRequestHeaders.Authorization.Scheme;
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(scheme, Token);
            HttpResponseMessage response = await Client.SendAsync(request);
            if (ensureSuccess)
            {
                response.EnsureSuccessStatusCode();
            }
            return response;
        }

        public TClient WithToken(string token)
        {
            Token = token;
            return ReturnClient();
        }

        public async Task<IEnumerable<T>> GetEntitiesAsync<T>(string url)
        {
            HttpResponseMessage response = await SendwithToken(HttpMethod.Get, url);
            return await Deserializer.DeserializeAsync<IEnumerable<T>>(response.Content);
        }

        public async Task<T> GetEntityAsync<T>(string url)
        {
            HttpResponseMessage response = await SendwithToken(HttpMethod.Get, url);
            return await Deserializer.DeserializeAsync<T>(response.Content);
        }

        public Task CreateEntityAsync<T>(T entity, string url)
        {
            string text = entity.ToString(); // TODO: serialize
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url) {
                Content = new StringContent(text)
            };
            return SendwithToken(request);
        }

        public Task DeleteEntityAsync(string url) => SendwithToken(HttpMethod.Delete, url);

        public Task UpdateEntityAsync<T>(T entity, string url)
        {
            string text = entity.ToString(); // TODO: serialize
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url) {
                Content = new StringContent(text)
            };
            return SendwithToken(request);
        }
    }
}
