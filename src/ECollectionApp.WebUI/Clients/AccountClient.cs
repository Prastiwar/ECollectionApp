using System.Net.Http;
using System.Threading.Tasks;

namespace ECollectionApp.WebUI.Clients
{
    public class AccountClient : IAccountClient
    {
        public AccountClient(HttpClient client) => Client = client;

        protected HttpClient Client { get; }

        public async Task<string> Login()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Api.Account.SignIn());
            HttpResponseMessage response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string token = await response.Content.ReadAsStringAsync();
            return token;
        }

        public async Task Logout()
        {
            HttpResponseMessage response = await Client.PostAsync(Api.Account.SignOut(), null);
            response.EnsureSuccessStatusCode();
        }
    }
}
