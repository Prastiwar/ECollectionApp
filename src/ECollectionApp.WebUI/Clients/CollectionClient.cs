using ECollectionApp.WebUI.Data;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECollectionApp.WebUI.Clients
{
    public class CollectionClient : ICollectionClient
    {
        public CollectionClient(HttpClient client) => Client = client;

        protected HttpClient Client { get; }

        protected string Token { get; set; }

        public async Task<IEnumerable<CollectionGroup>> GetGroupsAsync()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Api.Collection.GroupsUrl());
            string scheme = Client.DefaultRequestHeaders.Authorization.Scheme;
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(scheme, Token);
            HttpResponseMessage response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            using (System.IO.Stream responseStream = await response.Content.ReadAsStreamAsync())
            {
                // TODO: Use interface for serializator
                return await JsonSerializer.DeserializeAsync<IEnumerable<CollectionGroup>>(responseStream);
            }
        }

        public ICollectionClient WithToken(string token)
        {
            Token = token;
            return this;
        }
    }
}
