using ECollectionApp.WebUI.Serialization;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ECollectionApp.WebUI.Clients
{
    public abstract class TokenizedEntityClient<TClient> : ITokenizedClient<TClient>
        where TClient : ITokenizedClient<TClient>
    {
        public TokenizedEntityClient(HttpClient client, IHttpContentSerializationHandler serializer)
        {
            Client = client;
            Serializer = serializer;
        }

        protected HttpClient Client { get; }

        protected IHttpContentSerializationHandler Serializer { get; }

        protected string Token { get; set; }

        protected abstract TClient ReturnClient();

        protected virtual MediaTypeHeaderValue CreateMediaType() => new MediaTypeHeaderValue("application/json");

        protected Task<HttpResponseMessage> SendwithToken(HttpMethod method, string url, bool ensureSuccess = true)
            => SendwithToken(new HttpRequestMessage(method, url), ensureSuccess);

        protected async Task<HttpResponseMessage> SendwithToken(HttpRequestMessage request, bool ensureSuccess = true)
        {
            string scheme = Client.DefaultRequestHeaders.Authorization.Scheme;
            request.Headers.Authorization = new AuthenticationHeaderValue(scheme, Token);
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
            return await Serializer.DeserializeAsync<IEnumerable<T>>(response.Content);
        }

        public async Task<T> GetEntityAsync<T>(string url)
        {
            HttpResponseMessage response = await SendwithToken(HttpMethod.Get, url);
            return await Serializer.DeserializeAsync<T>(response.Content);
        }

        public async Task CreateEntityAsync<T>(T entity, string url)
        {
            HttpContent content = await Serializer.SerializeAsync(entity, typeof(T), CreateMediaType());
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url) {
                Content = content
            };
            await SendwithToken(request);
        }

        public Task DeleteEntityAsync(string url) => SendwithToken(HttpMethod.Delete, url);

        public async Task UpdateEntityAsync<T>(T entity, string url)
        {
            HttpContent content = await Serializer.SerializeAsync(entity, typeof(T), CreateMediaType());
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url) {
                Content = content
            };
            await SendwithToken(request);
        }
    }
}
