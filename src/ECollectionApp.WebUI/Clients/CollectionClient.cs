using ECollectionApp.WebUI.Data;
using ECollectionApp.WebUI.Serialization;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ECollectionApp.WebUI.Clients
{
    public class CollectionClient : ICollectionClient
    {
        public CollectionClient(HttpClient client, IHttpContentDeserializationHandler deserializer)
        {
            Client = client;
            Deserializer = deserializer;
        }

        protected HttpClient Client { get; }

        protected IHttpContentDeserializationHandler Deserializer { get; }

        protected string Token { get; set; }

        public async Task<IEnumerable<CollectionGroup>> GetGroupsAsync()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Api.Collection.GroupsUrl());
            string scheme = Client.DefaultRequestHeaders.Authorization.Scheme;
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(scheme, Token);
            HttpResponseMessage response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await Deserializer.DeserializeAsync<IEnumerable<CollectionGroup>>(response.Content);
        }

        public ICollectionClient WithToken(string token)
        {
            Token = token;
            return this;
        }
    }
}
