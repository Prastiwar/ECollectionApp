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

        public async Task<IEnumerable<CollectionGroup>> GetGroupsAsync(/*string token*/)
        {
            //string token = await HttpContext.GetTokenAsync(JwtBearerDefaults.AuthenticationScheme);
            //string token2 = User.Identity.Name;
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Api.Collection.GetGroups());
            //request.Headers.Add(JwtBearerDefaults.AuthenticationScheme, token);
            HttpResponseMessage response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            using (System.IO.Stream responseStream = await response.Content.ReadAsStreamAsync())
            {
                // TODO: Use interface for serializator
                return await JsonSerializer.DeserializeAsync<IEnumerable<CollectionGroup>>(responseStream);
            }
        }
    }
}
